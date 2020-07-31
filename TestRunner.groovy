#!groovy

echo "*** setting strings ***"
varEmailRecipients = "leonid.mishuk@idt.net,pavel.stryhelski+wwapi@idt.net,juan.aitken@idt.net"
statusSuccessful = "SUCCESS"
statusFailed = "FAILURE"
statusAborted = "ABORTED"
varProject = "WhiteWingsApi"
varCategory = "${CATEGORY}"
varEnvironment = "${ENVIRONMENT}"
varJobBuildNumber = "${env.BUILD_NUMBER}"
varJobName = "${env.JOB_NAME}"
varMilestoneId = "" // TODO
if(varCategory == 'psf_ww')
{
    varConcatCategory = "cat=='callback'"
} 
else 
{
    varConcatCategory = "cat=='WhiteWings' & cat=='${varCategory}'"
}

varBucketName = "idtq-boss-automation/WhiteWingsApi/"
pathToJob = "${env.BUILD_NUMBER}/${varProject}/bin/Debug"
varAllureResultsFolder = "${pathToJob}/allure-report"
varSlaveName = "windows_test_automation"
varTRId = "-1"
statusAllure = false

varReportLink = "http://idtq-automation.awsqa.idt.net/${varProject}/${varEnvironment}/${varCategory}/${varJobBuildNumber}/index.html"
env.slackChannel = "#ww_regression_report"
varMessage = "Automation for `${varProject}` - `${varEnvironment}` - `${varCategory}` #${env.BUILD_NUMBER} "
varTestRailTestRunLink = ""
varTestRailEmailLink = ""

varProjectId = "17"
varSuiteId = ""


varSlackReportsNotification = "Allure Report: (<http://idtq-automation.awsqa.idt.net/${varProject}/${varEnvironment}/${varCategory}/${varJobBuildNumber}/index.html|S3 Bucket>)"

echo "*** setting external files ***"
attachHelper = fileLoader.fromGit('CustomCopyAttachHelper', 'https://github.com/coretech/ww-api-automation.git', 'master', "${env.IDT_JENKINS_GITHUB_CREDENTIALS}", 'qa_linux_awscli')

currentBuild.description = varMessage

node("${varSlaveName}") {
    try {
        timestamps {

            stage('Copy and Unzip Automation Dll') {

                dir("${pathToJob}") {
                    attachHelper.copyAndUnzipAutomationDll(varBucketName)
                }
            }

            /* TODO
            stage('Test Rail Preparation') {
                dir("${pathToJob}") {
                    def varExploreCmd = "NUnit.ConsoleRunner.3.7.0\\tools\\nunit3-console WhiteWingsApi.dll --where \"${varConcatCategory}\" --explore > testRailIds.txt"
                    varTRId = createTestRun(varExploreCmd, varMessage, varProjectId.toInteger(), varSuiteId.toInteger())
                }
                def tRId = varTRId
                def mileId = varMilestoneId
                attachTestRailRunToAMilestone {
                    runId = tRId
                    milestoneId = mileId
                }

                varTestRailTestRunLink = "(<https://idt.testrail.com/index.php?/runs/view/${varTRId}|Test Rail Run>)"
                varTestRailEmailLink = "https://idt.testrail.com/index.php?/runs/view/${varTRId}"
            }

            mailToRecipients(varJobName, varJobBuildNumber, statusBuilding, varProject, varEmailRecipients, varEnvironment, varCategory, "", "")
            */

            def msg = varMessage
            sendSlackNotification {
                message = msg + "Started!"
                color = "#0c13f4"
            }

            stage('Run Tests') {

                echo "****** RUN TESTS ********"

                dir("${pathToJob}") {
                    bat "NUnit.ConsoleRunner.3.10.0\\tools\\nunit3-console WhiteWingsApi.dll --where \"${varConcatCategory}\" \"--result=TestResult.xml;format=nunit3\" --testparam=testrail_run_id=${varTRId}"
                }

                currentBuild.result = statusSuccessful

                mailToRecipients(varJobName, varJobBuildNumber, statusSuccessful, varProject, varEmailRecipients, varEnvironment, varCategory, varReportLink, varTestRailEmailLink)

                def vmsg = varMessage
                def msgAllure = varSlackReportsNotification
                def msgTestRail = varTestRailTestRunLink
                sendSlackNotification {
                    message = vmsg + "Successful!\n${msgAllure}\nTest Rail Run: ${msgTestRail}"
                }
            }
        }

    } catch (InterruptedException x) {

        currentBuild.result = statusAborted

        mailToRecipients(varJobName, varJobBuildNumber, statusAborted, varProject, varEmailRecipients, varEnvironment, varCategory, "", varTestRailEmailLink)

        def msg = varMessage
        def msgTestRail = varTestRailTestRunLink

        sendSlackNotification {
            message = msg + "Interrupted!\nTest Rail Run: ${msgTestRail}"
        }

        throw x

    } catch (error) {
        currentBuild.result = statusFailed

        mailToRecipients(varJobName, varJobBuildNumber, statusFailed, varProject, varEmailRecipients, varEnvironment, varCategory, varReportLink, varTestRailEmailLink)

        def msg = varMessage
        def msgAllure = varSlackReportsNotification
        def msgTestRail = varTestRailTestRunLink
        sendSlackNotification {
            message = msg + "Failed!\n${msgAllure}\nTest Rail Run: ${msgTestRail}"
        }

        throw error

    } finally {

        stage('Publish Allure Report') {
            try {
                dir("${pathToJob}") {
                    echo "****** Copy categories.json file into allure results folder ******"
                    bat "copy categories.json .\\allure-results"
                    if (fileExists('allure-results')) {
                        allure([
                                includeProperties: false,
                                jdk              : '',
                                properties       : [],
                                reportBuildPolicy: 'ALWAYS',
                                results          : [[path: 'allure-results']]
                        ])
                        statusAllure = true
                    }
                }
            } catch (allurePublishError) {
                echo "Something is wrong with Allure publishing. Error: " + allurePublishError
            }
        }

        stage('Copy Allure report to a Bucket') {

            echo "****** Copying report ********"
            if (statusAllure) {
                attachHelper.copyReports(varAllureResultsFolder, varProject, varEnvironment, varCategory, varJobBuildNumber)
            }
        }

        stage('Create Jira Ticket') {
            if (isJobTriggeredByTimer()) {
                try {
                    //JIRA Related stuff
                    def jiraHelper = fileLoader.fromGit('JiraHelper', 'https://github.com/coretech/br-bdd-automation.git', 'develop', "${env.IDT_JENKINS_GITHUB_CREDENTIALS}", 'qa_linux_awscli')

                    def summary = "Review for '${varProject}' - 'Regression' #${varJobBuildNumber}"
                    def description = "Daily Automation review.\nNeed to review a report by the link below:\n${varReportLink}"
                    def varJiraWatchers = 'kpowe'
                    def epicJira = "QEAUTO-3887"

                    jiraHelper.createDailyReviewTicket(summary, description, varJiraWatchers, epicJira, "whitewings")

                } catch (jiraError) {
                    echo "Something went wrong on Jira Creation. \nPlease see error: ${jiraError}"
                }
            }
        }

        /*stage('Close Test Rail Run') { TODO
            def tRId = varTRId
            closeTestRailRun {
                runId = tRId
            }
        } */

        stage('Copy Reports to Latest') {
            if (statusAllure) {
                try {
                    def repLink = varReportLink
                    uploadReportLinksToLatest {
                        link = repLink
                    }
                } catch (error) {
                    echo "Something went wrong on copying to latest. Error :: " + error
                }
            }
        }
    }
}

def testRailIds() {
    def fileContents = readFile('testRailIds.txt')
    def matches = (fileContents =~ /C\d+/)
    def listIds = []

    for (i in matches) {
        def element = i.toString().replaceAll('C', '').toInteger()
        listIds.push(element)
    }
    return listIds
}

def createTestRun(String varExplore, String varRunName, int varProjId, int varSuiteIds) {
    def varTRIds
    def listIds = []

        bat "${varExplore}"

        listIds = testRailIds()

        varTRIds = createTestRailRunWithAutoTestsOnly {
            runName = varRunName
            projectId = varProjId
            suiteId = varSuiteIds
            ids = listIds
        }

    echo "TestRail Run ID created is: '${varTRIds}'"
    return varTRIds
}

def mailToRecipients(String jobName, String jobBuildNumber, String buildStatus, String project, String emailRecipients, String environment, String category, String reportLink, String testRailLink) {

    def fullMessage = "'${jobName} - #${jobBuildNumber}' - ${buildStatus}.\nProject: ${project}\nEnvironment:'${environment}'\nCategory:'${category}'\nBuild details can be found at ${env.BUILD_URL}."

    if ("${reportLink}" != "") {
        fullMessage = fullMessage + "\nReport can be found by the link below:\n${reportLink}"
    }
    if ("${testRailLink}" != "") {
        fullMessage = fullMessage + "\nTest Rail Run link:\n${testRailLink}"
    }

    mail body: "${fullMessage}",
            subject: "'White Wings Automation' - '${project}': '${environment}'/'${category}' - (${jobBuildNumber}) - ${buildStatus}!",
            to: "${emailRecipients}"
}
