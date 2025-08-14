using dotnetapp.Exceptions;
using dotnetapp.Models;
using dotnetapp.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using System.Reflection;
using dotnetapp.Services;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace dotnetapp.Tests
{
    [TestFixture]
    public class Tests
    {

        private ApplicationDbContext _context; 
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            _context = new ApplicationDbContext(options);
           
             _httpClient = new HttpClient();
             _httpClient.BaseAddress = new Uri("http://localhost:8080");

        }

        [TearDown]
        public void TearDown()
        {
             _context.Dispose();
        }

   [Test, Order(1)]
    public async Task Backend_Test_Post_Method_Register_Admin_Returns_HttpStatusCode_OK()
    {
        ClearDatabase();
        string uniqueId = Guid.NewGuid().ToString();

        // Generate a unique userName based on a timestamp
        string uniqueUsername = $"abcd_{uniqueId}";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";

        string requestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"abc@123A\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\", \"UserRole\": \"Admin\"}}";
        HttpResponseMessage response = await _httpClient.PostAsync("/api/register", new StringContent(requestBody, Encoding.UTF8, "application/json"));

        Console.WriteLine(response.StatusCode);
        string responseString = await response.Content.ReadAsStringAsync();

        Console.WriteLine(responseString);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
  
   [Test, Order(2)]
    public async Task Backend_Test_Post_Method_Login_Admin_Returns_HttpStatusCode_OK()
    {
        ClearDatabase();

        string uniqueId = Guid.NewGuid().ToString();

        // Generate a unique userName based on a timestamp
        string uniqueUsername = $"abcd_{uniqueId}";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";

        string requestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"abc@123A\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\", \"UserRole\": \"Admin\"}}";
        HttpResponseMessage response = await _httpClient.PostAsync("/api/register", new StringContent(requestBody, Encoding.UTF8, "application/json"));

        // Print registration response
        string registerResponseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Registration Response: " + registerResponseBody);

        // Login with the registered user
        string loginRequestBody = $"{{\"Email\" : \"{uniqueEmail}\",\"Password\" : \"abc@123A\"}}"; // Updated variable names
        HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));

        // Print login response
        string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
        Console.WriteLine("Login Response: " + loginResponseBody);

        Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
    }


   [Test, Order(3)]
    public async Task Backend_Test_Post_Investment_With_Token_By_Admin_Returns_HttpStatusCode_OK()
    {
        ClearDatabase();
        string uniqueId = Guid.NewGuid().ToString();

        // Generate a unique userName based on a timestamp
        string uniqueUsername = $"abcd_{uniqueId}";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";

        string requestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"abc@123A\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\", \"UserRole\": \"Admin\"}}";
        HttpResponseMessage response = await _httpClient.PostAsync("/api/register", new StringContent(requestBody, Encoding.UTF8, "application/json"));

        // Print registration response
        string registerResponseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Registration Response: " + registerResponseBody);

        // Login with the registered user
        string loginRequestBody = $"{{\"Email\" : \"{uniqueEmail}\",\"Password\" : \"abc@123A\"}}"; // Updated variable names
        HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));

        // Print login response
        string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
        Console.WriteLine("Login Response: " + loginResponseBody);

        Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
        string responseBody = await loginResponse.Content.ReadAsStringAsync();

        dynamic responseMap = JsonConvert.DeserializeObject(responseBody);

        string token = responseMap.token;

        Assert.IsNotNull(token);

        string uniquetitle = Guid.NewGuid().ToString();

        // Use a dynamic and unique userName for admin (appending timestamp)
        string uniqueinvestmenttype = $"investment_{uniquetitle}";

        //  string investmentjson = $"{{\"InvestmentType\":\"{uniqueinvestmenttype}\",\"Description\":\"test\",\"InterestRate\":10,\"MaximumAmount\":1000,\"RepaymentTenure\":12,\"Eligibility\":\"Farmer with land ownership\",\"DocumentsRequired\":\"Land documents, ID proof, Income statement\"}}";

        string investmentjson = $"{{\"Name\":\"Mutual fund\",\"Description\":\"test\",\"ExpectedReturn\":10,\"RiskLevel\":\"Medium\",\"DurationInMonths\":12,\"MinimumInvestment\":1000,\"InvestmentType\":\"{uniqueinvestmenttype}\",\"DocumentsRequired\":\"Land documents, ID proof, Income statement\"}}";

        Console.WriteLine("Token: " + token);
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        HttpResponseMessage investmentresponse = await _httpClient.PostAsync("/api/investment",
        new StringContent(investmentjson, Encoding.UTF8, "application/json"));

       Console.WriteLine("investmentresponse"+investmentresponse);

        Assert.AreEqual(HttpStatusCode.OK, investmentresponse.StatusCode);
    }


    [Test, Order(4)]

    public async Task Backend_Test_Post_Investment_Without_Token_By_Admin_Returns_HttpStatusCode_Unauthorized()
    {
            ClearDatabase();
            string uniqueId = Guid.NewGuid().ToString();

            // Generate a unique userName based on a timestamp
            string uniqueUsername = $"abcd_{uniqueId}";
            string uniqueEmail = $"abcd{uniqueId}@gmail.com";

            string requestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"abc@123A\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\", \"UserRole\": \"Admin\"}}";
            HttpResponseMessage response = await _httpClient.PostAsync("/api/register", new StringContent(requestBody, Encoding.UTF8, "application/json"));

            // Print registration response
            string registerResponseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Registration Response: " + registerResponseBody);

            // Login with the registered user
            string loginRequestBody = $"{{\"Email\" : \"{uniqueEmail}\",\"Password\" : \"abc@123A\"}}"; // Updated variable names
            HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));

            // Print login response
            string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
            Console.WriteLine("Login Response: " + loginResponseBody);

            Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
    
            string uniquetitle = Guid.NewGuid().ToString();

            // Use a dynamic and unique userName for admin (appending timestamp)
            string uniqueinvestmenttype = $"investment_{uniquetitle}";

            string investmentjson = $"{{\"Name\":\"Mutual fund\",\"Description\":\"test\",\"ExpectedReturn\":10,\"RiskLevel\":\"Medium\",\"DurationInMonths\":12,\"MinimumInvestment\":1000,\"InvestmentType\":\"{uniqueinvestmenttype}\",\"DocumentsRequired\":\"Land documents, ID proof, Income statement\"}}";

            HttpResponseMessage investmentresponse = await _httpClient.PostAsync("/api/investment",
            new StringContent(investmentjson, Encoding.UTF8, "application/json"));

            Console.WriteLine("investmentresponse"+investmentresponse);

            Assert.AreEqual(HttpStatusCode.Unauthorized, investmentresponse.StatusCode);
    }


    [Test, Order(5)]
    public async Task Backend_Test_Get_Method_Get_InvestmentById_In_Investment_Service_Fetches_Investment_Successfully()
    {
        ClearDatabase();


    var investmentData = new Dictionary<string, object>
    {
        { "InvestmentId", 20 }, // Unique identifier for the investment plan
        { "Name", "Green Agriculture Plan" }, // Name of the investment plan
        { "Description", "A plan focused on sustainable agricultural investments." },
        { "ExpectedReturn", 7.5m }, // Expected annual return as a percentage
        { "RiskLevel", "Medium" }, // Risk level of the plan
        { "DurationInMonths", 24 }, // Duration of the plan in months
        { "MinimumInvestment", 100000m }, // Minimum investment amount required
        { "InvestmentType", "Agriculture" }, // Type of investment
        { "DocumentsRequired", "Land ownership proof, government ID, and bank statement" }
    };



        var investment = new Investment();
        foreach (var kvp in investmentData)
        {
            var propertyInfo = typeof(Investment).GetProperty(kvp.Key);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(investment, kvp.Value);
            }
        }
        _context.Investments.Add(investment);
        _context.SaveChanges();

        string assemblyName = "dotnetapp";
        Assembly assembly = Assembly.Load(assemblyName);
        string ServiceName = "dotnetapp.Services.InvestmentService";
        string typeName = "dotnetapp.Models.Investment";

        Type serviceType = assembly.GetType(ServiceName);
        Type modelType = assembly.GetType(typeName);


        MethodInfo getInvestmentMethod = serviceType.GetMethod("GetInvestmentById");
    

        if (getInvestmentMethod != null)
        {
            var service = Activator.CreateInstance(serviceType, _context);
            var retrievedInvestment = (Task<Investment>)getInvestmentMethod.Invoke(service, new object[] { 20 });

            Assert.IsNotNull(retrievedInvestment);
            Assert.AreEqual(investment.InvestmentType, retrievedInvestment.Result.InvestmentType);
        }
        else
        {
            Assert.Fail();
        }

    }

    [Test, Order(6)]
    public async Task Backend_Test_Put_Method_UpdateInvestment_In_Investment_Service_Updates_Investment_Successfully()
    {
        ClearDatabase();

    var investmentData = new Dictionary<string, object>
    {
        { "InvestmentId", 20 }, // Unique identifier for the investment plan
        { "Name", "Green Agriculture Plan" }, // Name of the investment plan
        { "Description", "A plan focused on sustainable agricultural investments." },
        { "ExpectedReturn", 7.5m }, // Expected annual return as a percentage
        { "RiskLevel", "Medium" }, // Risk level of the plan
        { "DurationInMonths", 24 }, // Duration of the plan in months
        { "MinimumInvestment", 100000m }, // Minimum investment amount required
        { "InvestmentType", "Agriculture" }, // Type of investment
        { "DocumentsRequired", "Land ownership proof, government ID, and bank statement" }
    };


        var investment = new Investment();
        foreach (var kvp in investmentData)
        {
            var propertyInfo = typeof(Investment).GetProperty(kvp.Key);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(investment, kvp.Value);
            }
        }
        _context.Investments.Add(investment);
        _context.SaveChanges();

        string assemblyName = "dotnetapp";
        Assembly assembly = Assembly.Load(assemblyName);
        string ServiceName = "dotnetapp.Services.InvestmentService";
        string typeName = "dotnetapp.Models.Investment";

        Type serviceType = assembly.GetType(ServiceName);
        Type modelType = assembly.GetType(typeName);


        MethodInfo updatemethod = serviceType.GetMethod("UpdateInvestment", new[] { typeof(int), modelType });


        if (updatemethod != null)
        {
            var service = Activator.CreateInstance(serviceType, _context);
            // Update the investment

            var updatedInvestmentData = new Dictionary<string, object>
        {
        { "InvestmentId", 20 }, // Unique identifier for the investment plan
        { "Name", "Green Agriculture Plan" }, // Name of the investment plan
        { "Description", "A plan focused on sustainable agricultural investments." },
        { "ExpectedReturn", 7.5m }, // Expected annual return as a percentage
        { "RiskLevel", "Medium" }, // Risk level of the plan
        { "DurationInMonths", 24 }, // Duration of the plan in months
        { "MinimumInvestment", 100000m }, // Minimum investment amount required
        { "InvestmentType", "Updated Agriculture" }, // Type of investment
        { "DocumentsRequired", "Land ownership proof, government ID, and bank statement" }
    };


            var updatedInvestment = Activator.CreateInstance(modelType);
            foreach (var kvp in updatedInvestmentData)
            {
                var propertyInfo = modelType.GetProperty(kvp.Key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(updatedInvestment, kvp.Value);
                }
            }

            var updateResult = (Task<bool>)updatemethod.Invoke(service, new object[] { 20, updatedInvestment });

            var updatedInvestmentFromDb = await _context.Investments.FindAsync(20);
            Assert.IsNotNull(updatedInvestmentFromDb);
            Assert.AreEqual("Updated Agriculture", updatedInvestmentFromDb.InvestmentType);

        }
        else
        {
            Assert.Fail();
        }   
    }

    [Test, Order(7)]
    public async Task Backend_Test_Delete_Method_DeleteInvestment_In_Investment_Service_Deletes_Investment_Successfully()
    {
        ClearDatabase();
        // Add investment
        var investmentData = new Dictionary<string, object>
    {
        { "InvestmentId", 4 }, // Unique identifier for the investment plan
        { "Name", "Green Agriculture Plan" }, // Name of the investment plan
        { "Description", "A plan focused on sustainable agricultural investments." },
        { "ExpectedReturn", 7.5m }, // Expected annual return as a percentage
        { "RiskLevel", "Medium" }, // Risk level of the plan
        { "DurationInMonths", 24 }, // Duration of the plan in months
        { "MinimumInvestment", 100000m }, // Minimum investment amount required
        { "InvestmentType", "Agriculture" }, // Type of investment
        { "DocumentsRequired", "Land ownership proof, government ID, and bank statement" }
    };


        var investment = new Investment();
        foreach (var kvp in investmentData)
        {
            var propertyInfo = typeof(Investment).GetProperty(kvp.Key);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(investment, kvp.Value);
            }
        }

        _context.Investments.Add(investment);
        _context.SaveChanges();

        string assemblyName = "dotnetapp";
        Assembly assembly = Assembly.Load(assemblyName);
        string ServiceName = "dotnetapp.Services.InvestmentService";
        string typeName = "dotnetapp.Models.Investment";

        Type serviceType = assembly.GetType(ServiceName);
        Type modelType = assembly.GetType(typeName);

        MethodInfo deletemethod = serviceType.GetMethod("DeleteInvestment", new[] { typeof(int) });

        if (deletemethod != null)
        {
            var service = Activator.CreateInstance(serviceType, _context);
            var deleteResult = (Task<bool>)deletemethod.Invoke(service, new object[] { 4 });

            var deletedInvestmentFromDb = await _context.Investments.FindAsync(4);
            Assert.IsNull(deletedInvestmentFromDb);
        }
        else
        {
            Assert.Fail();
        }
    }

    [Test, Order(8)]
    public async Task Backend_Test_Post_Method_AddInvestmentApplication_In_InvestmentApplication_Service_Posts_Successfully()
    {
        ClearDatabase();

        // Add user
        var userData = new Dictionary<string, object>
        {
            { "UserId", 400 },
            { "Username", "testuser" },
            { "Password", "testpassword" },
            { "Email", "test@example.com" },
            { "MobileNumber", "1234567890" },
            { "UserRole", "User" }
        };

        var user = new User();
        foreach (var kvp in userData)
        {
            var propertyInfo = typeof(User).GetProperty(kvp.Key);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(user, kvp.Value);
            }
        }
        _context.Users.Add(user);
        _context.SaveChanges();

    
        var investmentData = new Dictionary<string, object>
    {
            { "InvestmentId", 100 }, // Unique identifier for the investment plan
        { "Name", "Green Agriculture Plan" }, // Name of the investment plan
        { "Description", "A plan focused on sustainable agricultural investments." },
        { "ExpectedReturn", 7.5m }, // Expected annual return as a percentage
        { "RiskLevel", "Medium" }, // Risk level of the plan
        { "DurationInMonths", 24 }, // Duration of the plan in months
        { "MinimumInvestment", 100000m }, // Minimum investment amount required
        { "InvestmentType", "Agriculture" }, // Type of investment
        { "DocumentsRequired", "Land ownership proof, government ID, and bank statement" }
    };


        var investment = new Investment();
        foreach (var kvp in investmentData)
        {
            var propertyInfo = typeof(Investment).GetProperty(kvp.Key);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(investment, kvp.Value);
            }
        }
        _context.Investments.Add(investment);
        _context.SaveChanges();

        // Add investment application
        string assemblyName = "dotnetapp";
        Assembly assembly = Assembly.Load(assemblyName);
        string ServiceName = "dotnetapp.Services.InvestmentApplicationService";
        string typeName = "dotnetapp.Models.InvestmentApplication";

        Type serviceType = assembly.GetType(ServiceName);
        Type modelType = assembly.GetType(typeName);

        MethodInfo method = serviceType.GetMethod("AddInvestmentApplication", new[] { modelType });

        if (method != null)
        {
    

    var investmentApplicationData = new Dictionary<string, object>
    {
        { "InvestmentApplicationId", 200 },
        { "UserId", 400 },
        { "InvestmentId", 100 },
        { "InvestmentAmount", 5000m },
        { "ApplicationStatus", "Pending" },
        { "ApplicationDate", DateTime.Now },
        { "InvestmentReason", "To diversify portfolio" },
        { "InvestmentDuration", 24 },
        { "File", "investment_application.pdf" },
    };

            var investmentApplication = Activator.CreateInstance(modelType);
            foreach (var kvp in investmentApplicationData)
            {
                var propertyInfo = modelType.GetProperty(kvp.Key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(investmentApplication, kvp.Value);
                }
            }
            var service = Activator.CreateInstance(serviceType, _context);
            var result = (Task<bool>)method.Invoke(service, new object[] { investmentApplication });
        
            var addedInvestmentApplication = await _context.InvestmentApplications.FindAsync(200);
            Assert.IsNotNull(addedInvestmentApplication);
            Assert.AreEqual("investment_application.pdf",addedInvestmentApplication.File);

        }
        else{
            Assert.Fail();
        }
    }

    [Test, Order(9)]
    public async Task Backend_Test_Get_Method_GetInvestmentApplicationByUserId_In_InvestmentApplication_Fetches_Successfully()
    {
        // Add user
        ClearDatabase();

        var userData = new Dictionary<string, object>
        {
            { "UserId", 400 },
            { "Username", "testuser" },
            { "Password", "testpassword" },
            { "Email", "test@example.com" },
            { "MobileNumber", "1234567890" },
            { "UserRole", "User" }
        };

        var user = new User();
        foreach (var kvp in userData)
        {
            var propertyInfo = typeof(User).GetProperty(kvp.Key);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(user, kvp.Value);
            }
        }
        _context.Users.Add(user);
        _context.SaveChanges();


        var investmentData = new Dictionary<string, object>
        {
            { "InvestmentId", 100 },
            { "Name", "Green Agriculture Plan" },
            { "Description", "A plan focused on sustainable agricultural investments." },
            { "ExpectedReturn", 7.5m },
            { "RiskLevel", "Medium" },
            { "DurationInMonths", 24 },
            { "MinimumInvestment", 100000m },
            { "InvestmentType", "Agriculture" },
            { "DocumentsRequired", "Land ownership proof, government ID, and bank statement" }
        };

        var investment = new Investment();
        foreach (var kvp in investmentData)
        {
            var propertyInfo = typeof(Investment).GetProperty(kvp.Key);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(investment, kvp.Value);
            }
        }
        _context.Investments.Add(investment);
        _context.SaveChanges();

        var investmentApplicationData = new Dictionary<string, object>
        {
            { "InvestmentApplicationId", 200 },
            { "UserId", 400 },
            { "InvestmentId", 100 },
            { "InvestmentAmount", 5000m },
            { "ApplicationStatus", "Pending" },
            { "ApplicationDate", DateTime.Now },
            { "InvestmentReason", "To diversify portfolio" },
            { "InvestmentDuration", 24 },
            { "File", "investment_application.pdf" },
        };

        var investmentApplication = new InvestmentApplication();
        foreach (var kvp in investmentApplicationData)
        {
            var propertyInfo = typeof(InvestmentApplication).GetProperty(kvp.Key);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(investmentApplication, kvp.Value);
            }
        }
        _context.InvestmentApplications.Add(investmentApplication);
        _context.SaveChanges();

        // Add investment application
        string assemblyName = "dotnetapp";
        Assembly assembly = Assembly.Load(assemblyName);
        string ServiceName = "dotnetapp.Services.InvestmentApplicationService";
        string typeName = "dotnetapp.Models.InvestmentApplication";

        Type serviceType = assembly.GetType(ServiceName);
        Type modelType = assembly.GetType(typeName);

        MethodInfo method = serviceType.GetMethod("GetInvestmentApplicationsByUserId");

        if (method != null)
        {
            var service = Activator.CreateInstance(serviceType, _context);
            var result = (Task<IEnumerable<InvestmentApplication>>)method.Invoke(service, new object[] { 400 });
            Assert.IsNotNull(result);

            bool check = true;
            foreach (var item in result.Result)
            {
                Assert.AreEqual("To diversify portfolio", item.InvestmentReason);
                check = false;
            }

            if (check)
            {
                Assert.Fail();
            }
        }
        else
        {
            Assert.Fail();
        }
    }


[Test, Order(10)]

public async Task Backend_Test_Put_Method_Update_In_InvestmentApplication_Service_Updates_Successfully()
{
     ClearDatabase();

    var userData = new Dictionary<string, object>
    {
        { "UserId", 400 },
        { "Username", "testuser" },
        { "Password", "testpassword" },
        { "Email", "test@example.com" },
        { "MobileNumber", "1234567890" },
        { "UserRole", "User" }
    };

    var user = new User();
    foreach (var kvp in userData)
    {
        var propertyInfo = typeof(User).GetProperty(kvp.Key);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(user, kvp.Value);
        }
    }
    _context.Users.Add(user);
    _context.SaveChanges();


   

var investmentData = new Dictionary<string, object>
{
    { "InvestmentId", 100 }, // Unique identifier for the investment plan
    { "Name", "Green Agriculture Plan" }, // Name of the investment plan
    { "Description", "A plan focused on sustainable agricultural investments." },
    { "ExpectedReturn", 7.5m }, // Expected annual return as a percentage
    { "RiskLevel", "Medium" }, // Risk level of the plan
    { "DurationInMonths", 24 }, // Duration of the plan in months
    { "MinimumInvestment", 100000m }, // Minimum investment amount required
    { "InvestmentType", "Agriculture" }, // Type of investment
    { "DocumentsRequired", "Land ownership proof, government ID, and bank statement" }
};

    var investment = new Investment();
    foreach (var kvp in investmentData)
    {
        var propertyInfo = typeof(Investment).GetProperty(kvp.Key);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(investment, kvp.Value);
        }
    }
    _context.Investments.Add(investment);
    _context.SaveChanges();



var investmentApplicationData = new Dictionary<string, object>
{
    { "InvestmentApplicationId", 200 },
    { "UserId", 400 },
    { "InvestmentId", 100 },
    { "InvestmentAmount", 5000m },
    { "ApplicationStatus", "Pending" },
    { "ApplicationDate", DateTime.Now },
    { "InvestmentReason", "To diversify portfolio" },
    { "InvestmentDuration", 24 },
    { "File", "investment_application.pdf" },
};



    var investmentApplication = new InvestmentApplication();
     foreach (var kvp in investmentApplicationData)
    {
        var propertyInfo = typeof(InvestmentApplication).GetProperty(kvp.Key);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(investmentApplication, kvp.Value);
        }
    }
    _context.InvestmentApplications.Add(investmentApplication);
    _context.SaveChanges();

    // Add investment application
    string assemblyName = "dotnetapp";
    Assembly assembly = Assembly.Load(assemblyName);
    string ServiceName = "dotnetapp.Services.InvestmentApplicationService";
    string typeName = "dotnetapp.Models.InvestmentApplication";

    Type serviceType = assembly.GetType(ServiceName);
    Type modelType = assembly.GetType(typeName);

    MethodInfo method = serviceType.GetMethod("UpdateInvestmentApplication",new[] { typeof(int), modelType });

    if (method != null)
    {


        var updatedInvestmentApplicationData = new Dictionary<string, object>
{
    { "UserId", 400 },
    { "InvestmentId", 100 },
    { "InvestmentAmount", 5000m },
    { "ApplicationStatus", "Approved" },
    { "ApplicationDate", DateTime.Now },
    { "InvestmentReason", "To diversify portfolio" },
    { "InvestmentDuration", 24 },
    { "File", "investment_application.pdf" }

};

    var updatedInvestmentApplication = new InvestmentApplication();
    foreach (var kvp in updatedInvestmentApplicationData)
    {
        var propertyInfo = typeof(InvestmentApplication).GetProperty(kvp.Key);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(updatedInvestmentApplication, kvp.Value);
        }
    }

        var service = Activator.CreateInstance(serviceType, _context);
        var updateResult = (Task<bool>)method.Invoke(service, new object[] { 200, updatedInvestmentApplication });
        var updatedInvestmentFromDb = await _context.InvestmentApplications.FindAsync(200);
        Assert.IsNotNull(updatedInvestmentFromDb);
        Assert.AreEqual("Approved", updatedInvestmentFromDb.ApplicationStatus);   
    }
    else{
        Assert.Fail();
    }
}

[Test, Order(11)]
public async Task Backend_Test_Delete_Method_DeleteInvestmentApplication_Service_Deletes_InvestmentApplication_Successfully()
{
     ClearDatabase();

    var userData = new Dictionary<string, object>
    {
        { "UserId", 32 },
        { "Username", "testuser" },
        { "Password", "testpassword" },
        { "Email", "test@example.com" },
        { "MobileNumber", "1234567890" },
        { "UserRole", "User" }
    };

    var user = new User();
    foreach (var kvp in userData)
    {
        var propertyInfo = typeof(User).GetProperty(kvp.Key);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(user, kvp.Value);
        }
    }
    _context.Users.Add(user);
    _context.SaveChanges();




    var investmentData = new Dictionary<string, object>
{
    { "InvestmentId", 33}, // Unique identifier for the investment plan
    { "Name", "Green Agriculture Plan" }, // Name of the investment plan
    { "Description", "A plan focused on sustainable agricultural investments." },
    { "ExpectedReturn", 7.5m }, // Expected annual return as a percentage
    { "RiskLevel", "Medium" }, // Risk level of the plan
    { "DurationInMonths", 24 }, // Duration of the plan in months
    { "MinimumInvestment", 100000m }, // Minimum investment amount required
    { "InvestmentType", "Agriculture" }, // Type of investment
    { "DocumentsRequired", "Land ownership proof, government ID, and bank statement" }
};


    var investment = new Investment();
    foreach (var kvp in investmentData)
    {
        var propertyInfo = typeof(Investment).GetProperty(kvp.Key);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(investment, kvp.Value);
        }
    }
    _context.Investments.Add(investment);
    _context.SaveChanges();


   var investmentApplicationData = new Dictionary<string, object>
{
    { "InvestmentApplicationId", 200 },
    { "UserId", 32 },
    { "InvestmentId", 33 },
    { "InvestmentAmount", 5000m },
    { "ApplicationStatus", "Pending" },
    { "ApplicationDate", DateTime.Now },
    { "InvestmentReason", "To diversify portfolio" },
    { "InvestmentDuration", 24 },
    { "File", "investment_application.pdf" },
};

    var investmentApplication = new InvestmentApplication();
     foreach (var kvp in investmentApplicationData)
    {
        var propertyInfo = typeof(InvestmentApplication).GetProperty(kvp.Key);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(investmentApplication, kvp.Value);
        }
    }
    _context.InvestmentApplications.Add(investmentApplication);
    _context.SaveChanges();


    string assemblyName = "dotnetapp";
    Assembly assembly = Assembly.Load(assemblyName);
    string ServiceName = "dotnetapp.Services.InvestmentApplicationService";
    string typeName = "dotnetapp.Models.InvestmentApplication";

    Type serviceType = assembly.GetType(ServiceName);
    Type modelType = assembly.GetType(typeName);

    MethodInfo deletemethod = serviceType.GetMethod("DeleteInvestmentApplication", new[] { typeof(int) });

    if (deletemethod != null)
    {
        var service = Activator.CreateInstance(serviceType, _context);
        var deleteResult = (Task<bool>)deletemethod.Invoke(service, new object[] { 200 });

        var deletedInvestmentFromDb = await _context.InvestmentApplications.FindAsync(200);
        Assert.IsNull(deletedInvestmentFromDb);
    }
    else
    {
        Assert.Fail();
    }
     ClearDatabase();
}

[Test, Order(12)]
public async Task Backend_Test_Post_Method_AddFeedback_In_Feedback_Service_Posts_Successfully()
{
        ClearDatabase();

    // Add user
    var userData = new Dictionary<string, object>
    {
        { "UserId",42 },
        { "Username", "testuser" },
        { "Password", "testpassword" },
        { "Email", "test@example.com" },
        { "MobileNumber", "1234567890" },
        { "UserRole", "User" }
    };

    var user = new User();
    foreach (var kvp in userData)
    {
        var propertyInfo = typeof(User).GetProperty(kvp.Key);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(user, kvp.Value);
        }
    }
    _context.Users.Add(user);
    _context.SaveChanges();
    // Add investment application
    string assemblyName = "dotnetapp";
    Assembly assembly = Assembly.Load(assemblyName);
    string ServiceName = "dotnetapp.Services.FeedbackService";
    string typeName = "dotnetapp.Models.Feedback";

    Type serviceType = assembly.GetType(ServiceName);
    Type modelType = assembly.GetType(typeName);

    MethodInfo method = serviceType.GetMethod("AddFeedback", new[] { modelType });

    if (method != null)
    {
           var feedbackData = new Dictionary<string, object>
            {
                { "FeedbackId", 11 },
                { "UserId", 42 },
                { "FeedbackText", "Great experience!" },
                { "Date", DateTime.Now }
            };
        var feedback = new Feedback();
        foreach (var kvp in feedbackData)
        {
            var propertyInfo = typeof(Feedback).GetProperty(kvp.Key);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(feedback, kvp.Value);
            }
        }
        var service = Activator.CreateInstance(serviceType, _context);
        var result = (Task<bool>)method.Invoke(service, new object[] { feedback });
    
        var addedFeedback= await _context.Feedbacks.FindAsync(11);
        Assert.IsNotNull(addedFeedback);
        Assert.AreEqual("Great experience!",addedFeedback.FeedbackText);

    }
    else{
        Assert.Fail();
    }
}

[Test, Order(13)]
public async Task Backend_Test_Delete_Method_Feedback_In_Feeback_Service_Deletes_Successfully()
{
    // Add user
     ClearDatabase();

    var userData = new Dictionary<string, object>
    {
        { "UserId",42 },
        { "Username", "testuser" },
        { "Password", "testpassword" },
        { "Email", "test@example.com" },
        { "MobileNumber", "1234567890" },
        { "UserRole", "User" }
    };

    var user = new User();
    foreach (var kvp in userData)
    {
        var propertyInfo = typeof(User).GetProperty(kvp.Key);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(user, kvp.Value);
        }
    }
    _context.Users.Add(user);
    _context.SaveChanges();

           var feedbackData = new Dictionary<string, object>
            {
                { "FeedbackId", 11 },
                { "UserId", 42 },
                { "FeedbackText", "Great experience!" },
                { "Date", DateTime.Now }
            };
        var feedback = new Feedback();
        foreach (var kvp in feedbackData)
        {
            var propertyInfo = typeof(Feedback).GetProperty(kvp.Key);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(feedback, kvp.Value);
            }
        }
     _context.Feedbacks.Add(feedback);
    _context.SaveChanges();
    // Add investment application
    string assemblyName = "dotnetapp";
    Assembly assembly = Assembly.Load(assemblyName);
    string ServiceName = "dotnetapp.Services.FeedbackService";
    string typeName = "dotnetapp.Models.Feedback";

    Type serviceType = assembly.GetType(ServiceName);
    Type modelType = assembly.GetType(typeName);

  
    MethodInfo deletemethod = serviceType.GetMethod("DeleteFeedback", new[] { typeof(int) });

    if (deletemethod != null)
    {
        var service = Activator.CreateInstance(serviceType, _context);
        var deleteResult = (Task<bool>)deletemethod.Invoke(service, new object[] { 11 });

        var deletedFeedbackFromDb = await _context.Feedbacks.FindAsync(11);
        Assert.IsNull(deletedFeedbackFromDb);
    }
    else
    {
        Assert.Fail();
    }
}

[Test, Order(14)]
public async Task Backend_Test_Get_Method_GetFeedbacksByUserId_In_Feedback_Service_Fetches_Successfully()
{
    ClearDatabase();

    // Add user
    var userData = new Dictionary<string, object>
    {
        { "UserId", 330 },
        { "Username", "testuser" },
        { "Password", "testpassword" },
        { "Email", "test@example.com" },
        { "MobileNumber", "1234567890" },
        { "UserRole", "User" }
    };

    var user = new User();
    foreach (var kvp in userData)
    {
        var propertyInfo = typeof(User).GetProperty(kvp.Key);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(user, kvp.Value);
        }
    }
    _context.Users.Add(user);
    _context.SaveChanges();

    var feedbackData= new Dictionary<string, object>
    {
        { "FeedbackId", 13 },
        { "UserId", 330 },
        { "FeedbackText", "Great experience!" },
        { "Date", DateTime.Now }
    };

    var feedback = new Feedback();
    foreach (var kvp in feedbackData)
    {
        var propertyInfo = typeof(Feedback).GetProperty(kvp.Key);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(feedback, kvp.Value);
        }
    }
    _context.Feedbacks.Add(feedback);
    _context.SaveChanges();

    // Add investment application
    string assemblyName = "dotnetapp";
    Assembly assembly = Assembly.Load(assemblyName);
    string ServiceName = "dotnetapp.Services.FeedbackService";
    string typeName = "dotnetapp.Models.Feedback";

    Type serviceType = assembly.GetType(ServiceName);
    Type modelType = assembly.GetType(typeName);

    MethodInfo method = serviceType.GetMethod("GetFeedbacksByUserId");

    if (method != null)
    {
        var service = Activator.CreateInstance(serviceType, _context);
        var result = ( Task<IEnumerable<Feedback>>)method.Invoke(service, new object[] {330});
        Assert.IsNotNull(result);
         var check=true;
        foreach (var item in result.Result)
        {
            check=false;
            Assert.AreEqual("Great experience!", item.FeedbackText);
   
        }
        if(check==true)
        {
            Assert.Fail();

        }
    }
    else{
        Assert.Fail();
    }
}

//Exception
[Test, Order(15)]
 
public async Task Backend_Test_Post_Method_AddInvestment_In_InvestmentService_Occurs_InvestmentException_For_Duplicate_InvestmentType()
{
    ClearDatabase();

    string assemblyName = "dotnetapp";
    Assembly assembly = Assembly.Load(assemblyName);
    string ServiceName = "dotnetapp.Services.InvestmentService";
    string typeName = "dotnetapp.Models.Investment";
 
    Type serviceType = assembly.GetType(ServiceName);
    Type modelType = assembly.GetType(typeName);
 
    MethodInfo method = serviceType.GetMethod("AddInvestment", new[] { modelType });
 
    if (method != null)
    {
    
        var investmentData = new Dictionary<string, object>
    {
   { "InvestmentId", 2 }, // Unique identifier for the investment plan
    { "Name", "Green Agriculture Plan" }, // Name of the investment plan
    { "Description", "A plan focused on sustainable agricultural investments." },
    { "ExpectedReturn", 7.5m }, // Expected annual return as a percentage
    { "RiskLevel", "Medium" }, // Risk level of the plan
    { "DurationInMonths", 24 }, // Duration of the plan in months
    { "MinimumInvestment", 100000m }, // Minimum investment amount required
    { "InvestmentType", "Agriculture" }, // Type of investment
    { "DocumentsRequired", "Land ownership proof, government ID, and bank statement" }
   };

 
        var investment = Activator.CreateInstance(modelType);
        foreach (var kvp in investmentData)
        {
            var propertyInfo = modelType.GetProperty(kvp.Key);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(investment, kvp.Value);
            }
        }
 
        var service = Activator.CreateInstance(serviceType, _context);
        var result = (Task<bool>)method.Invoke(service, new object[] { investment });
        var addedInvestment = await _context.Investments.FindAsync(2);
        Assert.IsNotNull(addedInvestment);
   
        var investmentData1 = new Dictionary<string, object>
{
   { "InvestmentId", 3 }, // Unique identifier for the investment plan
    { "Name", "Green Agriculture Plan" }, // Name of the investment plan
    { "Description", "A plan focused on sustainable agricultural investments." },
    { "ExpectedReturn", 7.5m }, // Expected annual return as a percentage
    { "RiskLevel", "Medium" }, // Risk level of the plan
    { "DurationInMonths", 24 }, // Duration of the plan in months
    { "MinimumInvestment", 100000m }, // Minimum investment amount required
    { "InvestmentType", "Agriculture" }, // Type of investment
    { "DocumentsRequired", "Land ownership proof, government ID, and bank statement" }
};

 
        var investment1 = Activator.CreateInstance(modelType);
        foreach (var kvp in investmentData1)
        {
            var propertyInfo = modelType.GetProperty(kvp.Key);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(investment1, kvp.Value);
            }
        }
 
        try
        {
            var result1 = (Task<bool>)method.Invoke(service, new object[] { investment1 });
            Console.WriteLine("res"+result1.Result); 
            Assert.Fail();

        }
        catch (Exception ex)
        {

            Assert.IsNotNull(ex.InnerException);
            Assert.IsTrue(ex.InnerException is InvestmentException);
            Assert.AreEqual("Investment with the same type already exists", ex.InnerException.Message);
    }
    }
    else
    {
        Assert.Fail();
    }
   }
 

private void ClearDatabase()
{
    _context.Database.EnsureDeleted();
    _context.Database.EnsureCreated();
}

}
}