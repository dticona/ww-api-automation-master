echo "using standalone Attaching file for a job run!"

def copyReports(String pathToResults, String projectName, String environment, String category, String jobBuildNumber) {
    try {
        upload:
        {
            bat "aws s3 cp ./${pathToResults}/ s3://idtq-automation.awsqa.idt.net/${projectName}/${environment}/${category}/${jobBuildNumber}/ --recursive"
        }
    } catch (error) {
            echo "There was an error for report copying"
    }    
}

def copyAndUnzipAutomationDll(String varBucketName) {
    try {
        echo "Copying Automation Dll from '${varBucketName}' Bucket"
        copying:
        {
            bat "aws s3 cp s3://${varBucketName}WhiteWingsApi.zip ./"
        }

        echo "Unzipping Automation Dll"
        bat "\"C:\\Program Files\\7-Zip\\7z.exe\" x WhiteWingsApi.zip -aoa"
    } catch (error) {
        echo "There was an error with DLL copying from the S3 bucket"
        throw error
    }
}

return this