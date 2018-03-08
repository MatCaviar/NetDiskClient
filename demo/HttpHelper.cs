using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace demo
{
    class Ndutil
    {
        private static CookieContainer cookie;

        public static  void   login()
        {
            var name = "fengshaomin";
            var pwd = "jx6Q8oO5qiQ=";

            var getdata = "userName=fengshaomin&passWord=jx6Q8oO5qiQ=&method=login";
            var result = HttpGet("http://disk.bjsasc.com:8180/NetDisk/rest/mobile", getdata);
            var jsondata = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
            Properties.Settings.Default.token = jsondata["token"];
        }

        public static string upload(string filepath)
        {
            var filename = System.IO.Path.GetFileName(filepath);
            var extname = System.IO.Path.GetExtension(filepath);
            var postdata = new Dictionary<string, string>();
            postdata.Add("method", "formUpload");
            postdata.Add("parentFolderId", "");
            postdata.Add("fileName", filename);
            var filedata = new Dictionary<string, string>();
            filedata.Add(filename, filepath);
            var result = HttpPostFile("http://disk.bjsasc.com:8180/NetDisk/mine", postdata, filedata);
            var jsondata = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
            return "java";
        }
        public static List<NetDisk> get_list()
        {
            var getdata = "start=0&count=50&method=mobile_list";
            var result = HttpGet("http://disk.bjsasc.com:8180/NetDisk/rest/mobile", getdata);
            //JObject json1 = (JObject)JsonConvert.DeserializeObject(result);
            //JArray array = (JArray)json1["rows"];
            //List < NetDisk > li = new List<NetDisk>();
            Console.WriteLine(result);
            //var jsondata = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
            //var li = JsonConvert.DeserializeObject<List<NetDisk>>(jsondata["rows"]);

            var tokens = JObject.Parse(result);
            JArray array = (JArray)tokens["rows"];

            var a = new List<NetDisk>();

            foreach(var ob in array)
            {
                var o = (JObject)ob;
                var b = o.ToObject <NetDisk>();
                a.Add(b);
               

            }


            //获得日期范围过滤条件  

            return a;
        }

        public static string get_dowlload( string fileid,string id)
        {
            var getdata = string.Format("fileId={0}&id={1}&method=mobile_getDownloadAddr", fileid, id);
            var result = HttpGet("http://disk.bjsasc.com:8180/NetDisk/rest/mobile", getdata);
            //JObject json1 = (JObject)JsonConvert.DeserializeObject(result);
            //JArray array = (JArray)json1["rows"];
            //List < NetDisk > li = new List<NetDisk>();
            Console.WriteLine(result);
            //var jsondata = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
            //var li = JsonConvert.DeserializeObject<List<NetDisk>>(jsondata["rows"]);

            var tokens = JObject.Parse(result);
            string _result = (string)tokens["downloadurl"];

            return _result;
        }



        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="URL">下载文件地址</param>
        /// <param name="Filename">下载后的存放地址</param>
        /// <param name="Prog">用于显示的进度条</param>
        public static void DownloadFile(string URL, string filename, System.Windows.Forms.ProgressBar prog)
        {
         

            }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="URL">下载文件地址</param>
        /// <param name="Filename">下载后的存放地址</param>
        public static void DownloadFile(string URL, string filename)
        {
            DownloadFile(URL, filename, null);
        }
        private static string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            if (Properties.Settings.Default.token != "")
            {
                request.Headers["Authorization"] = Properties.Settings.Default.token;
            };
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public static string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            Console.WriteLine(request.RequestUri);
            request.ContentType = "text/html;charset=UTF-8";
            if (Properties.Settings.Default.token != "")
            {
                request.Headers["Authorization"] = Properties.Settings.Default.token;
            };
      
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
           
            return retString;


        }

        public static string HttpPostFile(string url, Dictionary<string, string> postData, Dictionary<string, string> files)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线  
            request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;

            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            Stream postStream = request.GetRequestStream();
            postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);

            //写入文本  
            if (postData != null && postData.Count > 0)
            {

                var keys = postData.Keys;
                foreach (var key in keys)
                {
                    string strHeader = string.Format("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n", key);
                    byte[] strByte = System.Text.Encoding.UTF8.GetBytes(strHeader);
                    postStream.Write(strByte, 0, strByte.Length);

                    byte[] value = System.Text.Encoding.UTF8.GetBytes(string.Format("{0}", postData[key]));
                    postStream.Write(value, 0, value.Length);

                    postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);

                }
            }
            postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);

            //写入文件  
            if (files != null && files.Count > 0)
            {
                var keys = files.Keys;

                foreach (var key in keys)
                {
                    string filePath = files[key];
                    int pos = filePath.LastIndexOf("\\");
                    string fileName = filePath.Substring(pos + 1);
                    StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"{0}\";filename=\"{1}\"\r\nContent-Type:application/octet-stream\r\n\r\n", key, fileName));
                    byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

                    FileStream fs = new FileStream(files[key], FileMode.Open, FileAccess.Read);
                    byte[] bArr = new byte[fs.Length];
                    fs.Read(bArr, 0, bArr.Length);
                    fs.Close();
                    postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                    postStream.Write(bArr, 0, bArr.Length);

                    postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                }

            }
      
            postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length); //结束标志  
            postStream.Close();
            //发送请求并获取相应回应数据  、
                       try
            {
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
          
            //直到request.GetResponse()程序才开始向目标网页发送Post请求  
            Stream instream = response.GetResponseStream();

            StreamReader sr = new StreamReader(instream, Encoding.UTF8);
            //返回结果网页（html）代码  
            string content = sr.ReadToEnd();
            return content;
            }
            catch (Exception)
            {

                return ("error");

            }


        }
    }
}
