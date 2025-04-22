using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace crm_site_tester.Tests;

[TestClass]
public class DemoTest : PageTest
{
    private IPlaywright _playwright;
    private IBrowser _browser;
    private IBrowserContext _browserContext;
    private IPage _page;
    
    [TestInitialize]
    public async Task Setup()
    {
        _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
            SlowMo = 1000
        });
        _browserContext = await _browser.NewContextAsync();
        _page = await _browserContext.NewPageAsync();
    }

    [TestCleanup]
    public async Task Cleanup()
    {
        await _browserContext.CloseAsync();
        await _browser.CloseAsync();
        _playwright.Dispose();
    }

    [TestMethod]
    public async Task LoginAsAdminTest()
    {
        await _page.GotoAsync("http://localhost:3000/");
        await _page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("m@email.com");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("abc123");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
    }
    
    [TestMethod]
    public async Task LogInAsAdminAndChangeTheStatusOfAnIssueTest()
    {
        await _page.GotoAsync("http://localhost:3000/");
        await _page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("m@email.com");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("abc123");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await _page.GetByRole(AriaRole.Link, new() { Name = "Issues" }).ClickAsync();
        await _page.Locator("div").Filter(new() { HasTextRegex = new Regex("^OPEN✎$") }).GetByRole(AriaRole.Button).ClickAsync();
        await _page.GetByRole(AriaRole.Combobox).SelectOptionAsync(new[] { "CLOSED" });
        await _page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
    }
    
    [TestMethod]
    public async Task CreateAnIssueAsANonSignedInUserTest()
    {
        await _page.GotoAsync("http://localhost:3000/");
        await _page.GetByRole(AriaRole.Link, new() { Name = "Demo AB" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Your email" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Your email" }).FillAsync("test@email.com");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Title" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Title" }).FillAsync("Test: GUI-001");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Message" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Message" }).FillAsync("Test: GUI-001");
        void Page_Dialog1_EventHandler(object sender, IDialog dialog)
        {
            Console.WriteLine($"Dialog message: {dialog.Message}");
            dialog.DismissAsync();
            _page.Dialog -= Page_Dialog1_EventHandler;
        }
        _page.Dialog += Page_Dialog1_EventHandler;
        await _page.GetByRole(AriaRole.Button, new() { Name = "Create issue" }).ClickAsync();
    }

    [TestMethod]
    public async Task CreateAnIssueAsASignedInUserTest()
    {
        await _page.GotoAsync("http://localhost:3000/");
        await _page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("no@email.com");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("abc123");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await _page.GetByRole(AriaRole.Link, new() { Name = "Demo AB" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Your email" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Your email" }).FillAsync("no@email.com");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Title" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Title" }).FillAsync("Test: GUI-002");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Message" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Message" }).FillAsync("Test: GUI-002");
        void Page_Dialog2_EventHandler(object sender, IDialog dialog)
        {
            Console.WriteLine($"Dialog message: {dialog.Message}");
            dialog.DismissAsync();
            _page.Dialog -= Page_Dialog2_EventHandler;
        }
        _page.Dialog += Page_Dialog2_EventHandler;
        await _page.GetByRole(AriaRole.Button, new() { Name = "Create issue" }).ClickAsync();
    }

    [TestMethod]
    public async Task MyTest()
    {
        await _page.GotoAsync("http://localhost:3000/");
        await _page.GetByRole(AriaRole.Link, new() { Name = "Register" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("example@email.com");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("acb123");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Username" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Username" }).FillAsync("ExampleName");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Company" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Company" }).FillAsync("Example Company");
        void Page_Dialog_EventHandler(object sender, IDialog dialog)
        {
            Console.WriteLine($"Dialog message: {dialog.Message}");
            dialog.DismissAsync();
            _page.Dialog -= Page_Dialog_EventHandler;
        }
        _page.Dialog += Page_Dialog_EventHandler;
        await _page.GetByRole(AriaRole.Button, new() { Name = "Skapa konto" }).ClickAsync();
        await _page.GotoAsync("http://localhost:3000/login");
    }
}