using Amazon;
using Amazon.Rekognition;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace ChatNikolya
{

    public class ReadJsonFromS3
    {
        string accessKey = "AKIAV5NLL37YZIQQTRRV";
        string secretKey = "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";

        string bucketName = "nikolyabucket";
        //string objectKey = NameSearch;
        string objectKey = "halla7.png";


        public ReadJsonFromS3()
        {


        }

        public async Task<string> ReadJsonFileFromS3(string objectKey)
        {
            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            // Create an Amazon S3 client with your credentials
            var s3Client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USEast2);

            // Create an Amazon Rekognition client with your credentials
            var rekognitionClient = new AmazonRekognitionClient(credentials, Amazon.RegionEndpoint.USEast1);
            
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = objectKey
            };
            using var response = await s3Client.GetObjectAsync(request);
            using var reader = new StreamReader(response.ResponseStream);
            return await reader.ReadToEndAsync();
        }

        public string ExtractObjectFromJson(string jsonContent)
        {
            dynamic json = JsonConvert.DeserializeObject(jsonContent);
            //dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(json);
            string transcript = json["results"]["transcripts"][0]["transcript"];
            return transcript;
        }
    }
}
