using Amazon.S3;
using System.Windows.Forms;
using Amazon.S3.Model;
using Amazon;
using Amazon.Rekognition.Model;
using Amazon.Rekognition;
using System.Drawing.Imaging;
using Image = System.Drawing.Image;
using Amazon.Runtime;
using System.Diagnostics.SymbolStore;
using System.Reflection.Emit;
using System.Linq;

using OpenAI_API;
using OpenAI_API.Completions;
using Amazon.TranscribeService.Model;
using Amazon.TranscribeService;

namespace ChatNikolya
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private static readonly string bucketName = "nikolyabucket";
        private static string keyName = "imgface.png";
        private static readonly string accessKey = "AKIAV5NLL37Y36O5Q43I";
        private static readonly string secretKey = "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";
        private static readonly RegionEndpoint regionEndpoint = RegionEndpoint.USEast2;



        private async Task UploadToS3Async(string filePath)
        {

            try
            {
                var client = new AmazonS3Client(accessKey, secretKey, regionEndpoint);
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = Path.GetFileName(filePath),
                    FilePath = filePath
                };
                var response = await client.PutObjectAsync(putRequest);
                //await ExampleAsync(Path.GetFileName(filePath));
                //MessageBox.Show($"Upload completed. Request Id: {response.ResponseMetadata.RequestId}");
            }
            catch (AmazonS3Exception ex)
            {
                MessageBox.Show($"Error encountered ***. Message:'{ex.Message}' when uploading an object");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unknown error encountered on server. Message:'{ex.Message}' when uploading an object");
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            //var fileDialog = new OpenFileDialog();
            //if (fileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    string filePath = fileDialog.FileName;
            //    await UploadToS3Async(filePath);
            //    await ExampleAsync(Path.GetFileName(filePath));
            //}

            //await UploadToS3Async("s");
            
        }

        public async Task ExampleAsync(string odjName)
        {
            string accessKey = "AKIAV5NLL37YZIQQTRRV";
            string secretKey = "0hXfq12bNOkoUs7TJAtPoNBiVRiFkgoawbg9RGTh";

            string bucketName = "nikolyabucket";
            //string objectKey = NameSearch;
            string objectKey = odjName;

            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            // Create an Amazon S3 client with your credentials
            var s3Client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USEast1);

            // Create an Amazon Rekognition client with your credentials
            var rekognitionClient = new AmazonRekognitionClient(credentials, Amazon.RegionEndpoint.USEast1);

            // Create a GetObjectRequest object to download the image from S3
            var getObjectRequest = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = objectKey
            };
            Console.WriteLine("start");
            // Call the GetObject API to download the image
            var getObjectResponse = await s3Client.GetObjectAsync(getObjectRequest);

            // Read the image data from the response stream
            byte[] imageBytes;
            using (var responseStream = getObjectResponse.ResponseStream)
            {
                using (var memoryStream = new MemoryStream())
                {
                    responseStream.CopyTo(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }
            }

            // Create a DetectText request object
            var detectTextRequest = new DetectTextRequest
            {
                Image = new Amazon.Rekognition.Model.Image
                {
                    Bytes = new MemoryStream(imageBytes)
                }
            };
            List<string> foundText = new List<string>(); // список найденных совпадений
            string output = "";
            // Call the DetectText API
            var detectTextResponse = await rekognitionClient.DetectTextAsync(detectTextRequest);

            // Print the detected text
            foreach (var textDetection in detectTextResponse.TextDetections)
            {
                if (!output.Contains(textDetection.DetectedText))
                {
                    output += textDetection.DetectedText + "\n"; // добавляем уникальное совпадение в вывод
                }
                //label2.Text += textDetection.DetectedText;

                //Console.WriteLine($"Detected text: {textDetection.DetectedText}");
                Console.WriteLine("end");
            }
            label2.Text = output;
            await SearchWithTextImg(output,label3);
        }
        public async Task SearchWithTextImg(string searchText, System.Windows.Forms.Label label)
        {
            string apiKey = "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";
            string answer = string.Empty;
            var openai = new OpenAIAPI(apiKey);
            CompletionRequest completion = new CompletionRequest();
            completion.Prompt = searchText;
            completion.Model = OpenAI_API.Models.Model.DavinciText;
            completion.MaxTokens = 4000;
            var result = await openai.Completions.CreateCompletionAsync(completion); 

            if (result != null)
            {
                foreach (var item in result.Completions)
                {
                    answer = item.Text;
                    label.Text += item.Text;
                }
            }
            

        }
        public async Task UploadWav(string searchText, System.Windows.Forms.Label label)
        {
            string apiKey = "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";
            string answer = string.Empty;
            var openai = new OpenAIAPI(apiKey);
            CompletionRequest completion = new CompletionRequest();
            completion.Prompt = searchText;
            completion.Model = OpenAI_API.Models.Model.DavinciText;
            completion.MaxTokens = 4000;
            var result = await openai.Completions.CreateCompletionAsync(completion);

            if (result != null)
            {
                foreach (var item in result.Completions)
                {
                    answer = item.Text;
                    label.Text += item.Text;
                }
            }


        }

        private AudioRecorder _audioRecorder;
        string nameRecFile;
        string PathRecFile;
        private void button2_Click(object sender, EventArgs e)
        {
            //_audioRecorder = new AudioRecorder(Path.Combine(Application.StartupPath, "Recordings"));
            //MessageBox.Show(Path.Combine(Application.StartupPath, "Recordings"));
            //_audioRecorder.StartRecording();
            
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            //nameRecFile = _audioRecorder.FileNames;
            //PathRecFile = _audioRecorder.PathToFile;
            //MessageBox.Show(nameRecFile);
            //_audioRecorder.StopRecording();
            //_audioRecorder.Dispose();
            //_audioRecorder = null;
            //await UploadToS3Async(PathRecFile);
            //label5.Text = await TranscribeAudio("s3://nikolyabucket/" + nameRecFile, "en-US");

        }
        string nameTranscribe;
        
        public async Task<string> TranscribeAudio(string filePath, string languageCode)
        {
            string accessKey = "AKIAV5NLL37YZIQQTRRV";
            string secretKey = "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";

            string bucketName = "nikolyabucket";
            //string objectKey = NameSearch;

            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            // Create an Amazon S3 client with your credentials
            var s3Client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USEast2);
            nameTranscribe = $"{DateTime.Now:yyyyMMddHHmmss}";
            // Create an Amazon Rekognition client with your credentials
            var rekognitionClient = new AmazonTranscribeServiceClient(credentials, Amazon.RegionEndpoint.USEast2);
            var request = new StartTranscriptionJobRequest
            {
                LanguageCode = languageCode,
                Media = new Media
                {
                    MediaFileUri = filePath
                },
                MediaFormat = MediaFormat.Wav,
                OutputBucketName = bucketName, // Замените на имя своего бакета
                TranscriptionJobName = nameTranscribe, // Уникальное имя задачи
        };

            var response = await rekognitionClient.StartTranscriptionJobAsync(request);
            return response.TranscriptionJob.TranscriptionJobStatus.Value;
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            
           //label5.Text = await TranscribeAudio("s3://nikolyabucket/"+ nameRecFile, "en-US");
        }
        ReadJsonFromS3 reader = new ReadJsonFromS3();
        private async void button5_Click(object sender, EventArgs e)
        {

            //string s3FilePath = nameTranscribe + ".json";
            //string jsonContent = await reader.ReadJsonFileFromS3(s3FilePath);


            //string infoFromJson = reader.ExtractObjectFromJson(jsonContent);
            ////MessageBox.Show(myObject);
            //label6.Text = infoFromJson;
            //await SearchWithTextImg(infoFromJson, label7);

        }

        private async void pictureBox1_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            await SearchWithTextImg(textBox1.Text, label1);
        }

        private async void pictureBox2_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = fileDialog.FileName;
                await UploadToS3Async(filePath);
                await ExampleAsync(Path.GetFileName(filePath));
            }

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            _audioRecorder = new AudioRecorder(Path.Combine(Application.StartupPath, "Recordings"));
            //MessageBox.Show(Path.Combine(Application.StartupPath, "Recordings"));
            _audioRecorder.StartRecording();
        }

        private async void pictureBox4_Click(object sender, EventArgs e)
        {
            nameRecFile = _audioRecorder.FileNames;
            PathRecFile = _audioRecorder.PathToFile;
            //MessageBox.Show(nameRecFile);
            _audioRecorder.StopRecording();
            _audioRecorder.Dispose();
            _audioRecorder = null;
            await UploadToS3Async(PathRecFile);
            label5.Text = await TranscribeAudio("s3://nikolyabucket/" + nameRecFile, "en-US");
            MessageBox.Show("Запись успешно загружена!\nОжидайте 10 секунд для обработки! ");
        }

        private async void pictureBox5_Click(object sender, EventArgs e)
        {

            string s3FilePath = nameTranscribe + ".json";
            string jsonContent = await reader.ReadJsonFileFromS3(s3FilePath);


            string infoFromJson = reader.ExtractObjectFromJson(jsonContent);
            //MessageBox.Show(myObject);
            label12.Text = infoFromJson;
            await SearchWithTextImg(infoFromJson, label7);
        }
    }
}