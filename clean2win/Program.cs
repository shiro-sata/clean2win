//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//                          READ ME
//      Si un bug est trouvé, qu'un changement est à effectuer
//           ou que vous avez n'importe quel soucis,
//             un repos github est disponnible
//                           ici :
//          https://github.com/shiro-sata/clean2win
//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

class Program
{
    //-----------------------importation des DLL importantes-----------------------
    //la librairie shell32 permet d'executer les fonctions de l'api windows. ça peut permettre pluisieurs choses, mais dans ce cas ça permet de vider la corbeille.
    [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
    static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);
    //importation de la DLL user32 (pour la MessageBox)
    [DllImport("user32.dll", SetLastError = true)]
    static extern int MessageBox(IntPtr h, string m, string c, int type);
    [return: MarshalAs(UnmanagedType.Bool)]
    //importation de la DLL user32 (pour changer le fond d'écran)
    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SystemParametersInfo(uint uiAction, uint uiParam, String pvParam, uint fWinIni);
    private const uint SPI_SETDESKWALLPAPER = 0x14;
    private const uint SPIF_UPDATEINIFILE = 0x1;
    private const uint SPIF_SENDWININICHANGE = 0x2;
    [return: MarshalAs(UnmanagedType.Bool)]

    /* 
     --documentations utile--
    Forum sur lequel je me suis aidé pour shell32.dll (vider la corbeille)     : https://stackoverflow.com/questions/19969418/how-to-programatically-clear-the-recycle-bin-under-windows
    Forum sur lequel je me suis aidé pour user32.dll (changer le fond d'écran) : https://www.codeproject.com/Questions/1252479/How-do-I-change-the-desktop-background-using-Cshar
    pour le MessageBox je me suis aidé d'un autre projet à moi, donc pas de documentation.

    !!! le fond d'écran ne se change pas définitivement !!
    -------------------------
     */
    //----------------------------------------------------------------------------

    //parti du code géré par shell32.dll.
    enum RecycleFlags : uint
    {
        SHERB_NOCONFIRMATION = 0x00000001,
        SHERB_NOPROGRESSUI = 0x00000002,
        SHERB_NOSOUND = 0x00000004
    }


    static void Main(string[] args)
    {
        if(args.Length == 0)  //Si il n'y a aucun argument spécifié (exemple : clean2win.exe -h)
        {          
            string username = Environment.UserName; //variable indiquant au programme l'utilisateur qui l'exécute
            Console.WriteLine("Préparations de votre ordinateur, veuillez ne pas fermer cette fenêtre.");  //écrire dans la Console        
            string photo = @"C:\Windows\Web\4K\Wallpaper\Windows\img0_3840x2160.jpg";  //variable vers le chemin du fond d'écran par défaut windows
            uint result = SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlags.SHERB_NOCONFIRMATION); //vider la corbeille
            DisplayPicture(photo);  //appel de la fonction DisplayPicture
            DesktopFolder(); //appel de la fonction DesktopFolder 
            DownloadFolder();  //appel de la fonction DownloadFolder         
            DocFolder();   //appel de la fonction DocFolder          
            ImageFolder();   //appel de la fonction ImageFolder           
            MessageBox((IntPtr)0, "votre ordinateur est prêt ! \n \n pour plus d'information sur le logiciel, ouvrez l'invte de commandes \n dans le repertoire C:/Program Files et tapper la commande suivant \n clean2win.exe -i", "Info", 0);  //affiche le MessageBox
        }
        if (args.Length > 0)  //si il y a un argument specifier
        {
            string firstArgument = args[0].ToLower(); // Convertir en minuscules pour la comparaison insensible à la casse

            switch (firstArgument)
            {             
                case "-h": //si l'argument "-h" est spécifier
                    
                    ShowHelp(); //applze la fonction showHelp
                    break;
                
                case "-i": //si l'argument "-h" est spécifier
                    
                    ShowInfo(); //appler la fonction ShowInfo

                    break;
 
                default: //si l'argument spécifier n'est pas bon
                    
                    Console.WriteLine("Commande non reconnue. Utilisez -help pour obtenir de l'aide."); //écrire dans la Console le message d'erreur
                    break;
            }
        }
    }
    public static void DownloadFolder() //fonction pour vider le dossier telechargement
    {
        string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");  //variable spécifiant le chemin vers le dossier telechargement

        DirectoryInfo downloadDirectory = new DirectoryInfo(downloadsPath);

        if (downloadDirectory.Exists)
        {
            //supprimer les fichiers
            foreach (FileInfo file in downloadDirectory.EnumerateFiles())
            {
                try
                {
                    file.Delete();
                }
                catch
                {
                    //si il y a une erreur, alors écrire catched dans la console
                    Console.WriteLine("Catched");
                    
                }

            }
            //supprimer les dossiers
            foreach (DirectoryInfo dir in downloadDirectory.EnumerateDirectories())
            {
                try
                {
                    dir.Delete(true);
                }
                catch
                {
                    //si il y a une erreur, alors écrire catched dans la console
                    Console.WriteLine("Catched");
                }

            }
        }

    }
    //fonction du dossier documents
    public static void DocFolder() //fonction pour vider le dossier documents
    {
        string DocPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents"); //variable spécifiant le chemin vers le dossier documents

        DirectoryInfo downloadDirectory = new DirectoryInfo(DocPath);

        if (downloadDirectory.Exists)
        {
            //supprimer les fichiers
            foreach (FileInfo file in downloadDirectory.EnumerateFiles())
            {

                if (!file.FullName.Contains("Visual Studio 2022")) // ne pas effacer le dossier ""visual Studio 2022"
                {
                    try
                    {
                        file.Delete();
                    }
                    catch
                    {
                        //si il y a une erreur, alors écrire catched dans la console
                        Console.WriteLine("Catched");
                    }

                }

            }
            //supprimer les dossiers
            foreach (DirectoryInfo dir in downloadDirectory.EnumerateDirectories())
            {
                
                if (!dir.FullName.Contains("Visual Studio 2022")) //ne pas effacer le dossier "Visual Studio 2022"
                {
                    try
                    {
                        dir.Delete(true);
                    }
                    catch
                    {
                        //si il y a une erreur, alors écrire catched dans la console
                        Console.WriteLine("Catched");
                    }

                }

            }
        }
    }
    //fonction du bureau
    public static void DesktopFolder() //fonction pour vider le bureau
    {       
        string desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)); //variable spécifiant le chemin vers le bureau

        DirectoryInfo desktopDirectory = new DirectoryInfo(desktopPath);

        if (desktopDirectory.Exists)
        {
            //supprimer les fichiers
            foreach (FileInfo file in desktopDirectory.EnumerateFiles())
            {
                if (file.Extension.ToLower() != ".lnk") // ne pas effacer les fichiers avec l'extension .link
                {
                    try
                    {
                        file.Delete();
                    }
                    catch
                    {
                        //si il y a une erreur, alors écrire catched dans la console
                        Console.WriteLine("Catched");
                    }

                }
            }
            //supprimer les dossiers
            foreach (DirectoryInfo dir in desktopDirectory.EnumerateDirectories()) 
            {

                if (dir.Extension.ToLower() != ".lnk" && !dir.FullName.Contains("exclude")) // ne pas effacer les fichiers avec l'extension .link et le dossier "exclude"
                {
                    try
                    {
                        dir.Delete(true);
                    }
                    catch
                    {
                        //si il y a une erreur, alors écrire catched dans la console
                        Console.WriteLine("Catched");
                    }

                }
            }
        }
    }
    public static void ImageFolder() //fonction pour vider le dossier Image
    {
        string ImageFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Pictures"); //variable spécifiant le chemin vers le dossier photos

        DirectoryInfo downloadDirectory = new DirectoryInfo(ImageFolder);

        if (downloadDirectory.Exists)
        {
            //supprimer les fichiers
            foreach (FileInfo file in downloadDirectory.EnumerateFiles())
            {
                try
                {
                    file.Delete();
                }
                catch
                    {
                    //si il y a une erreur, alors écrire catched dans la console
                    Console.WriteLine("Catched");
                }
                

            }
            // supprimer les dossiers
            foreach (DirectoryInfo dir in downloadDirectory.EnumerateDirectories())
            {
                try
                {
                    dir.Delete(true);
                }
                catch
                    {
                    //si il y a une erreur, alors écrire catched dans la console
                    Console.WriteLine("Catched");
                }
                

            }
        }

    }
    //fonction privée permettant de changer le fond d'écran
    private static void DisplayPicture(string file_name)
    {
        uint flags = 0;
        if (!SystemParametersInfo(SPI_SETDESKWALLPAPER,
                0, file_name, flags))
        {
            Console.WriteLine("Error");
        }
    }

   //Fonction pour afficher l'aide
    static void ShowHelp()
    {
        Console.WriteLine("========================Help===========================\r\n\r\n\t\t\t-h\t\r\n\t\taffiche cette page\r\n\r\n\t\t\t-i\t\r\n\taffiche les infos sur le logiciel\r\n\r\n=======================================================");
        
       
    }
    //fonction pour afficher l'info
    static void ShowInfo()
    {
        Console.WriteLine("========================Info===========================\r\nClean2win est un projet open source developper par shiro-sata.\r\n\r\nla page github : https://github.com/shiro-sata/clean2win \r\n\r\nsi vous avez besoin de modifier le programme vous pouvez !\r\nce projet est fait pour être modifier.\r\n\r\nLa commande help : clean2win.exe -h\r\nversion : 1.1\r\n=======================================================");
    }
}
