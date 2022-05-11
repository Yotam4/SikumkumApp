using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SikumkumApp.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;


namespace SikumkumApp.Services
{
    class SikumkumAPIProxy
    {
        private const string CLOUD_URL = "TBD"; //API url when going on the cloud
        private const string CLOUD_PHOTOS_URL = "TBD";
        private const string DEV_ANDROID_EMULATOR_URL = "http://10.0.2.2:60047/SikumkumAPIController"; //API url when using emulator on android
        private const string DEV_ANDROID_PHYSICAL_URL = "http://192.168.1.14:60047/SikumkumAPIController"; //API url when using physucal device on android
        private const string DEV_WINDOWS_URL = "https://localhost:44390/SikumkumAPIController"; //API url when using windoes on development
        private const string DEV_ANDROID_EMULATOR_PHOTOS_URL = "http://10.0.2.2:60047/images/"; //API url when using emulator on android
        private const string DEV_ANDROID_EMULATOR_PDFS_URL = "http://10.0.2.2:60047/pdfs/"; //API url when using emulator on android

        private const string DEV_ANDROID_PHYSICAL_PHOTOS_URL = "http://192.168.1.14:60047/Images/"; //API url when using physucal device on android
        private const string DEV_WINDOWS_PHOTOS_URL = "https://localhost:44390/Images/"; //API url when using windoes on development

        private HttpClient client;
        public string baseUri;
        public string basePhotosUri;
        public string basePdfsUri;
        private static SikumkumAPIProxy proxy = null;

        public static SikumkumAPIProxy CreateProxy() //Added base Pdfs uri, not sure it is needed.
        {
            string baseUri;
            string basePhotosUri;
            string basePdfUri = "";
            if (App.IsDevEnv)
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    if (DeviceInfo.DeviceType == DeviceType.Virtual)
                    {
                        baseUri = DEV_ANDROID_EMULATOR_URL;
                        basePhotosUri = DEV_ANDROID_EMULATOR_PHOTOS_URL;
                    }
                    else
                    {
                        baseUri = DEV_ANDROID_PHYSICAL_URL;
                        basePhotosUri = DEV_ANDROID_PHYSICAL_PHOTOS_URL;
                    }
                }
                else
                {
                    baseUri = DEV_WINDOWS_URL;
                    basePhotosUri = DEV_WINDOWS_PHOTOS_URL;
                }
            }
            else //Should not enter here when debugging, check with Ofer. Work in progress.
            {
                baseUri = DEV_ANDROID_EMULATOR_URL; //Using android 
                basePhotosUri = DEV_ANDROID_EMULATOR_PHOTOS_URL;
                basePdfUri = DEV_ANDROID_EMULATOR_PDFS_URL;
                //baseUri = CLOUD_URL;
                //basePhotosUri = CLOUD_PHOTOS_URL;
            }

            if (proxy == null)
                proxy = new SikumkumAPIProxy(baseUri, basePhotosUri, basePdfUri);
            return proxy;
        }


        private SikumkumAPIProxy(string baseUri, string basePhotosUri, string basePdfsUri)
        {
            //Set client handler to support cookies!!
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new System.Net.CookieContainer();

            //Create client with the handler!
            this.client = new HttpClient(handler, true);
            this.baseUri = baseUri;
            this.basePhotosUri = basePhotosUri;
            this.basePdfsUri = basePdfsUri;
        }

        //public string GetBasePhotoUri() { return this.basePhotosUri; }

        //Login!
        public async Task<User> LoginAsync(string username, string pass)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/Login?username={username}&pass={pass}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    User u = JsonSerializer.Deserialize<User>(content, options);
                    return u;
                }

                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public async Task<bool> LogoutAsync(User loggingOut)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };

                string jsonUser = JsonSerializer.Serialize<User>(loggingOut, options);
                StringContent content = new StringContent(jsonUser, Encoding.UTF8, "application/json");


                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/Logout", content);

                if (response.IsSuccessStatusCode) //If user sucessfully logged out up.
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch
            {
                return false;
            }
        }

        public async Task<bool> SignUpAsync(User signingUp)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };

                string jsonUser = JsonSerializer.Serialize<User>(signingUp, options);
                StringContent content = new StringContent(jsonUser, Encoding.UTF8, "application/json");


                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/SignUp", content);

                if (response.IsSuccessStatusCode) //If user sucessfully signed up.
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch
            {
                return false;
            }
        }

        public async Task<OpeningObject> GetOpeningObject()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetOpeningObject");

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    OpeningObject openingOb = JsonSerializer.Deserialize<OpeningObject>(content, options);
                    return openingOb;
                }
                return null;
            }

            catch (Exception e)
            {
                string error = e.Message;
                return null;
            }
        }

        public async Task<List<SikumFile>> GetSikumFiles(bool getSummary, bool getEssay, bool getPractice, string subjectName, int yearID, string headlineSearch)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetFiles?getSummary={getSummary}&getEssay={getEssay}&getPractice={getPractice}&subjectName={subjectName}&yearID={yearID}&headlineSearch={headlineSearch}");
                if (response.StatusCode == System.Net.HttpStatusCode.OK) //Returned more than one file.
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve,
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                        PropertyNameCaseInsensitive = true
                    };

                    string content = await response.Content.ReadAsStringAsync();
                    List<SikumFile> files = JsonSerializer.Deserialize<List<SikumFile>>(content, options);
                    return files;
                }
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent) //No files were found.
                {
                    return null;
                }

                return null;
            }

            catch (Exception e)
            {
                string a = e.Message;
                return null;
            }
        }

        public async Task<List<SikumFile>> GetUserSikumFiles(User u, int isAppr)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetUserFiles?userID={u.UserID}&isApproved={isAppr}");
                if (response.StatusCode == System.Net.HttpStatusCode.OK) 
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve,
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                        PropertyNameCaseInsensitive = true
                    };

                    string content = await response.Content.ReadAsStringAsync();
                    List<SikumFile> userFiles = JsonSerializer.Deserialize<List<SikumFile>>(content, options);
                    return userFiles;
                }
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent) //No files were found.
                {
                    return null;
                }

                return null;
            }

            catch (Exception e)
            {
                string a = e.Message;
                return null;
            }
        }

        public async Task<bool> TryChangePassword(User u, string newPassword)
        {
            try
            {
                if (u.Password == newPassword)
                    return false;
                User newPassUser = new User(u.Username, u.Email, newPassword);
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };

                string changePassString = JsonSerializer.Serialize<User>(newPassUser, options);
                StringContent content = new StringContent(changePassString, Encoding.UTF8, "application/json");


                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/ChangePassword", content);

                if (response.IsSuccessStatusCode) //If user sucessfully changed password
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch
            {
                return false;
            }

        }

        public async Task<bool> UploadSikumFile(SikumFile uploadSikum)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };

                string jsonUser = JsonSerializer.Serialize<SikumFile>(uploadSikum, options);
                StringContent content = new StringContent(jsonUser, Encoding.UTF8, "application/json");


                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/UploadSikumFile", content);

                if (response.IsSuccessStatusCode) //If user sucessfully signed up.
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch
            {
                return false;
            }
        }   

        //Upload files to server
        public async Task<bool> UploadFiles(Models.FileInfo[] fileInfoFiles, string targetFileName, string contentType)
        {
            try
            {
                var multipartFormDataContent = new MultipartFormDataContent();

                for (int i = 0; i < fileInfoFiles.Length; i++) //Adds files to multipart.
                {
                    var fileContent = new ByteArrayContent(File.ReadAllBytes(fileInfoFiles[i].Name));
                    multipartFormDataContent.Add(fileContent, "file", $"{targetFileName}{i+1}");
                }
                HttpResponseMessage response = null; //Sets default null value.

                if (contentType == "image")
                     response = await client.PostAsync($"{this.baseUri}/UploadImages", multipartFormDataContent); //Posts images


                if (contentType == "pdf")
                    response = await client.PostAsync($"{this.baseUri}/UploadPdfs", multipartFormDataContent); //Posts images
                
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public async Task<string> DownloadPdfFileAsync(string url, string fileName) //Downloads from servr file to AppDataDirectory
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            if (File.Exists(filePath))
                return filePath;

            var pdfBytes = await this.client.GetByteArrayAsync(url);

            try
            {
                File.WriteAllBytes(filePath, pdfBytes);

                return filePath;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }

            return null;
        }

        public async Task<bool> TryAcceptUpload(SikumFile sikum)
        {
            try
            {

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };

                string sikumFileJson = JsonSerializer.Serialize<SikumFile>(sikum, options);
                StringContent content = new StringContent(sikumFileJson, Encoding.UTF8, "application/json");


                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/TryAcceptUpload", content);

                if (response.IsSuccessStatusCode) //If sikumfile was sucessfully accepted.
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public async Task<bool> TryRejectUpload(SikumFile sikum)
        {
            try
            {

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };

                string sikumFileJson = JsonSerializer.Serialize<SikumFile>(sikum, options);
                StringContent content = new StringContent(sikumFileJson, Encoding.UTF8, "application/json");


                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/TryRejectUpload", content);

                if (response.IsSuccessStatusCode) //If sikumfile was sucessfully accepted.
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<List<SikumFile>> GetPendingFiles()
        {
                try
                {
                    HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetPendingFiles");
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) //Returned more than one file.
                    {
                        JsonSerializerOptions options = new JsonSerializerOptions
                        {
                            ReferenceHandler = ReferenceHandler.Preserve,
                            Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                            PropertyNameCaseInsensitive = true
                        };

                        string content = await response.Content.ReadAsStringAsync();
                        List<SikumFile> pendingFiles = JsonSerializer.Deserialize<List<SikumFile>>(content, options);
                        return pendingFiles;
                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent) //No files were found.
                    {
                        return null;
                    }

                    return null;
                }

                catch (Exception e)
                {
                    string a = e.Message;
                    return null;
                }
        }

        public async Task<bool> TryDeleteSikum(SikumFile sikum)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };

                string jsonUser = JsonSerializer.Serialize<SikumFile>(sikum, options);
                StringContent content = new StringContent(jsonUser, Encoding.UTF8, "application/json");


                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/DeleteSikumFile", content);

                if (response.IsSuccessStatusCode) //If user sucessfully signed up.
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch
            {
                return false;
            }
        }
        public async Task<double> RateSikum(Rating rating) //Change to boolean? Work in progress.
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };

                string jsonRating = JsonSerializer.Serialize<Rating>(rating, options);
                StringContent content = new StringContent(jsonRating, Encoding.UTF8, "application/json");


                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/AddRating", content);

                string returnContent = await response.Content.ReadAsStringAsync();
                double returnRating = double.Parse(returnContent); //Function returns double.

                if (response.IsSuccessStatusCode) //If user sucessfully signed up.
                {
                    return returnRating;
                }
                else
                {
                    return -1.00;
                }
            }

            catch
            {
                return -1.00;
            }
        }

        public async Task<bool> AddMessage(Message msg)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };

                string jsonMsg = JsonSerializer.Serialize<Message>(msg, options);
                StringContent content = new StringContent(jsonMsg, Encoding.UTF8, "application/json");


                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/AddMessage", content);

                if (response.IsSuccessStatusCode) //If message was added
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch
            {
                return false;
            }
        }
        public async Task<List<Message>> GetMessages(int fileId)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetMessages?fileID={fileId}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    List<Message> messages = JsonSerializer.Deserialize<List<Message>>(content, options);
                    return messages;
                }

                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }


    }

}