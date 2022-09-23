using MenuSystem;

Console.WriteLine("Hello, World!");

var mainMenu = new Menu(EMenuLevel.Main, "Main Menu", null);

mainMenu.RunForUserInput();