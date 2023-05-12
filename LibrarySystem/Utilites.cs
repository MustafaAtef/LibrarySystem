using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilties {
    public class Messages {
         public static void PrintMessage(string message, MessagesStatus messageStatus = MessagesStatus.Default) {
            int messageLength = message.Length;
            if (messageStatus == MessagesStatus.Error) Console.ForegroundColor = ConsoleColor.Red;
            else if (messageStatus == MessagesStatus.Success) Console.ForegroundColor = ConsoleColor.Green;
            else if (messageStatus == MessagesStatus.Exit) Console.ForegroundColor = ConsoleColor.DarkBlue;
            else if (messageStatus == MessagesStatus.Intro) Console.ForegroundColor = ConsoleColor.Cyan;
            for (int i = 0; i < messageLength+4; i++) Console.Write("-");
            Console.WriteLine();
            Console.WriteLine($"- {message } -");
            for (int i = 0; i < messageLength+4; i++) Console.Write("-");
            Console.WriteLine();
            Console.ResetColor();
         }
    }

    public enum MessagesStatus {
        Default,
        Success,
        Error,
        Exit,
        Intro
    }
}
