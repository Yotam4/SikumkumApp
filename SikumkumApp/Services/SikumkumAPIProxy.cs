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
        private const string DEV_ANDROID_EMULATOR_PHOTOS_URL = "http://10.0.2.2:60047/Images/"; //API url when using emulator on android
        private const string DEV_ANDROID_PHYSICAL_PHOTOS_URL = "http://192.168.1.14:60047/Images/"; //API url when using physucal device on android
        private const string DEV_WINDOWS_PHOTOS_URL = "https://localhost:44390/Images/"; //API url when using windoes on development

        private HttpClient client;
        private string baseUri;
        private string basePhotosUri;
        private static SikumkumAPIProxy proxy = null;

        public static SikumkumAPIProxy CreateProxy()
        {
            string baseUri;
            string basePhotosUri;
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
            else
            {
                baseUri = DEV_ANDROID_EMULATOR_URL; //Using android 
                //baseUri = CLOUD_URL;
                basePhotosUri = CLOUD_PHOTOS_URL;
            }

            if (proxy == null)
                proxy = new SikumkumAPIProxy(baseUri, basePhotosUri);
            return proxy;
        }


        private SikumkumAPIProxy(string baseUri, string basePhotosUri)
        {
            //Set client handler to support cookies!!
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new System.Net.CookieContainer();

            //Create client with the handler!
            this.client = new HttpClient(handler, true);
            this.baseUri = baseUri;
            this.basePhotosUri = basePhotosUri;
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
                        PropertyNameCaseInsensitive = false
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

        public async Task<bool> SignUpAsync(User signingUp)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = false
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

        public async Task<List<Subject>> GetSubjects()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetSubjects");

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };

                string content = await response.Content.ReadAsStringAsync();
                List<Subject> subjects = JsonSerializer.Deserialize<List<Subject>>(content, options);
                return subjects;
            }

            catch
            {
                return null;
            }
        }

        public async Task<List<SikumFile>> GetSikumFiles(bool getSummary, bool getEssay, bool getPractice, string subjectName)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetFiles?getSummary={getSummary}&getEssay={getEssay}&getPractice={getPractice}&subjectName={subjectName}");
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

        public async Task<List<SikumFile>> GetUserSikumFiles(string username)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetUserFiles?username={username}");
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
                    PropertyNameCaseInsensitive = false
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
    }
}