using SpecFlow.Test.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TXC.Common.Domain;
using Xunit;

namespace SpecFlow.Test.Steps
{
    [Binding]
    public class ApiSteps
    {
        private string _env;

        public ApiSteps()
        {
            _env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower();
        }

        [Given(@"Environment (.*)")]
        public void GivenEnvironment(string env)
        {
            DataManager.GetData().Environment = env;
        }

        [Given(@"The parameter (.*) '(.*)'")]
        public void GivenTheParameter(string parameterName, string parameterValue)
        {
            DataManager.GetData().parameters[parameterName] = parameterValue;
            DataManager.GetData().parameters[parameterName.ToLower()] = parameterValue;
        }

        [Then(@"The response message is (.*)")]
        public void ThenTheResponseMessage(string responseMessageExpected)
        {
            string ResponseMessage = DataManager.GetData().ResponseMessage;
            //Assert.Equal(responseMessageExpected.ToLower(), DataManager.GetData().ResponseMessage.ToLower());
        }

        [Then(@"The response success is (.*)")]
        public void ThenTheResponseSuccess(string responseSuccessExpected)
        {
            Assert.Equal(bool.Parse(responseSuccessExpected), DataManager.GetData().ResponseSuccess);
        }

        [Then(@"The response must contain (.*)")]
        public void ThenTheResponseMustContain(string expectedString)
        {
            Assert.True(DataManager.GetData().MustContain.Contains(expectedString));
        }

        [Then(@"The number of records is (.*)")]
        public void ThenTheNumberOfRecords(int expectedNum)
        {
            Assert.Equal(DataManager.GetData().NumberOfRecords, expectedNum);
        }

        [Then(@"The new Id is (.*)")]
        public void ThenTheNewId(int expectedNum)
        {
            Assert.Equal(DataManager.GetData().ReturnId, expectedNum);
        }

        public static void ExecuteAPI<T>(T request, HttpMethod method) where T : MediatR.IBaseRequest, new()
        {
            try
            {
                if (request == null && method != HttpMethod.Get)
                {
                    request = SetRequest<T>();
                }

                var _httpClient = new System.Net.Http.HttpClient();

                if (DataManager.GetData().parameters.ContainsKey("TenantName"))
                    _httpClient.DefaultRequestHeaders.Add(HeaderConstants.TenantName, DataManager.GetData().parameters["TenantName"]);

                if (DataManager.GetData().parameters.ContainsKey("TenantBasicInfoId"))
                    _httpClient.DefaultRequestHeaders.Add(HeaderConstants.TenantId, DataManager.GetData().parameters["TenantBasicInfoId"]);

                if (DataManager.GetData().parameters.ContainsKey("TX2UserName"))
                    _httpClient.DefaultRequestHeaders.Add(HeaderConstants.TX2UserName, DataManager.GetData().parameters["TX2UserName"].ToString());

                System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();

                Uri uri = new Uri(DataManager.GetData().URL);

                if (method == HttpMethod.Get)
                {
                    Type type = typeof(T);
                    var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite);

                    var dic = new Dictionary<string, string>();
                    foreach (var item in DataManager.GetData().parameters)
                    {
                        if (properties.Any(p => p.Name == item.Key))
                        {
                            dic[item.Key] = item.Value;
                        }
                    }

                    uri = new Uri(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(DataManager.GetData().URL, dic));
                    response = _httpClient.GetAsync(uri).Result;
                    response.EnsureSuccessStatusCode();
                }
                else
                {
                    // [FromBody] format
                    var serializeData = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                    var jsonContent = new System.Net.Http.StringContent(serializeData, System.Text.Encoding.UTF8, "application/json");

                    if (method == HttpMethod.Put)
                    {
                        response = _httpClient.PutAsync(uri, jsonContent).Result;
                    }
                    else if (method == HttpMethod.Post)
                    {
                        response = _httpClient.PostAsync(uri, jsonContent).Result;
                    }

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        response.EnsureSuccessStatusCode();
                    }
                }

                var content = response.Content.ReadAsStringAsync().Result;
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Response<object>>(content);

                DataManager.GetData().ResponseSuccess = result.Success;
                DataManager.GetData().ResponseMessage = result.Message;
            }
            catch (Exception ex)
            {
                DataManager.GetData().ResponseSuccess = false;
                DataManager.GetData().ResponseMessage = ex.Message;
            }
        }

        public static T SetRequest<T>() where T : MediatR.IBaseRequest, new()
        {
            var obj = new T();

            Type type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite);

            foreach (PropertyInfo property in properties)
            {
                if (DataManager.GetData().parameters.ContainsKey(property.Name))
                {
                    string value = DataManager.GetData().parameters[property.Name];

                    if (value == null)
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(value))
                    {
                        if (property.PropertyType.Name == "String")
                        {
                            property.SetValue(obj, value);
                        }

                        continue;
                    }

                    try
                    {
                        if (property.PropertyType.IsEnum)
                        {
                            property.SetValue(obj, Convert.ChangeType(value, Enum.GetUnderlyingType(property.PropertyType)));
                        }
                        else
                        {
                            property.SetValue(obj, Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType));
                        }
                    }
                    catch { }
                }
            }

            return obj;
        }
    }
}
