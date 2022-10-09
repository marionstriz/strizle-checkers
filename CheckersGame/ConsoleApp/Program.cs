using MenuSystem;
using static ConsoleUI.ConsoleUI;

Console.WriteLine("Hello, World!");

var thirdMenu = new Menu(EMenuLevel.MoreThanSecond, "Third Menu", null);

var secondMenuItems = new List<MenuItem> {new ("3", "Go To Third Menu", () => RunMenuForUserInput(thirdMenu))};
var secondMenu = new Menu(EMenuLevel.Second, "Second Menu", secondMenuItems);

var mainMenuItems = new List<MenuItem> {new ("2", "Go To Second Menu", () => RunMenuForUserInput(secondMenu))};
var mainMenu = new Menu(EMenuLevel.Main, "Main Menu", mainMenuItems);


RunMenuForUserInput(mainMenu);