﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CoRenpy {
    public partial class Main : Form {
        public Main() {
            InitializeComponent();
            //StartPrintScreen();
        }

        private const int _identSpaces = 4;


        private void processPath_Click(object sender, EventArgs e) {
            var dir = textBox2.Text;
            ProcessDir(dir);
        }

        private void processText_Click(object sender, EventArgs e) {
            textBox1.Text = ProcessFile(textBox1.Text);
        }

        private void ProcessDir(string dir) {
            if (Directory.Exists(dir)) {
                foreach (var fileDir in Directory.GetFiles(dir)) {
                    if (new FileInfo(fileDir).Extension == ".rpy") {
                        var text = File.ReadAllText(fileDir);
                        File.WriteAllText(fileDir, ProcessFile(text));
                    }
                }

                foreach (var directory in Directory.GetDirectories(dir)) {
                    ProcessDir(directory);
                }
            }
        }

        public string ProcessFile(string textFile) {
            string[] textLines = textFile.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            textLines = textLines.RemoveEmpty();

            List<string> lines = new List<string>();
            Type thisType = Type.Empty;
            int thisIdent = 0;
            List<string> listOfMods = new List<string>();

            foreach (var line in textLines) {
                var lastType = thisType;
                var lastIdent = thisIdent;
                var thisLine = line.Trim('\t', ' ');
                thisType = GetType(thisLine);
                thisIdent = GetIdent(line.Replace("\t", GetIdented(1)));

                if (thisType == Type.DecompilerComment)
                    continue;

                if ((thisType.In(Type.Renpy, Type.Operator, Type.Menu, Type.MenuItem) && !lastType.In(Type.Renpy, Type.Operator, Type.Menu) && lines.Count() > 0) || // Jump Before Stars or Operators or Menus or Menu Items
                    (thisType == Type.MenuItem && lastType == Type.Talk) || // Jump Before MenuItem if it has Text
                    (thisIdent < lastIdent && !thisType.In(Type.ImageExtender) && !lastType.In(Type.Operator, Type.Menu, Type.MenuItem)) || // Jump after you reduce the ident
                    (thisType == Type.Send)) // Jump before sending somewhere
                    lines.Add("");

                if (thisType.In(Type.Operator, Type.Variable)) {
                    if (thisLine.StartsWith("$")) {
                        var nextChar = thisLine[1];

                        if (nextChar != ' ')
                            thisLine = thisLine.Substring(0, 1) + " " + thisLine.Substring(1);
                    }

                    var symbolPos = TestSymbols(thisLine, 0, '=', '>', '<', '+', '-', '*', '/');
                    var symbolLast = -1;

                    while (symbolPos > symbolLast) {
                        var nextPos = symbolPos + 1;
                        var lastPos = symbolPos - 1;
                        var nextChar = thisLine[nextPos];
                        var lastChar = thisLine[lastPos];
                        symbolLast = symbolPos;

                        if (nextChar == '=') {
                            nextChar = thisLine[nextPos += 1];
                            symbolLast++;
                        }

                        if (lastChar.In('=', '<', '!', '>', '+', '-', '/', '*'))
                            lastChar = thisLine[lastPos -= 1];

                        if (nextChar != ' ')
                            thisLine = thisLine.Substring(0, nextPos) + " " + thisLine.Substring(nextPos);

                        if (lastChar != ' ') {
                            thisLine = thisLine.Substring(0, lastPos + 1) + " " + thisLine.Substring(lastPos + 1);
                            symbolLast++;
                        }

                        symbolPos = TestSymbols(thisLine, symbolLast + 1, '=', '>', '<', '+', '-', '*', '/');
                    }
                }

                if (thisType == Type.Renpy && thisLine.StartsWith("image ") && cMod.Checked) {
                    var tempName = thisLine.Substring(thisLine.IndexOf("image ") + 6, thisLine.IndexOf("=") - 6).Trim();

                    if (thisLine.IndexOf(" =") > 0)
                        thisLine = thisLine.Substring(0, thisLine.IndexOf(" =")) + "_mod" + thisLine.Substring(thisLine.IndexOf(" ="));
                    else
                        thisLine = thisLine.Substring(0, thisLine.IndexOf("=")) + "_mod " + thisLine.Substring(thisLine.IndexOf("="));

                    listOfMods.Add($"    '{tempName}': '{tempName}_mod',");
                }

                if (thisType == Type.Closing && thisIdent == lastIdent && thisIdent > 0)
                    thisIdent--;

                lines.Add(GetIdented(thisIdent) + thisLine);
            }
            
			var newLine = Environment.NewLine;
            return string.Join(newLine, lines) + newLine + string.Join(newLine, listOfMods);
        }

        private Type GetType(string line) {
            if (line == "")
                return Type.Empty;

            if (line.StartsWithAny("init", "python", "class", "def "))
                return Type.Python;

            if (line.StartsWithAny("play", "pause", "$ renpy.", "$renpy."))
                return Type.RenpyFn;

            if (line.StartsWithAny("add", "scene", "show", "hide"))
                return Type.Image;

            if (line.StartsWithAny(
                    "with", "parallel", "linear", "ypos", "xpos", "xalign", "yalign", "repeat",
                    "anchor", "pos", "ycenter", "xcenter"
                )
            )
                return Type.ImageExtender;

            if (line.StartsWithAny("call", "jump", "return"))
                return Type.Send;

            if (line.StartsWithAny("if", "elif", "else", "while", "for"))
                return Type.Operator;

            if (line.StartsWithAny(
                    "screen", "label", "transform", "hbox", "vbox", "window", "has", "text",
                    "button", "style", "frame", "grid", "imagemap", "imagebutton", "on", "image",
                    "draggroup"
                )
            )
                return Type.Renpy;

            if (line.StartsWith("$"))
                return Type.Variable;

            if (line.StartsWith("menu:"))
                return Type.Menu;

            if (line.StartsWith("\"") && line.EndsWith(":"))
                return Type.MenuItem;

            if (line.StartsWith("# Decompiled by unrpyc"))
                return Type.DecompilerComment;

            if (line.StartsWith("#"))
                return Type.Comment;

            if (line.Last() == ':')
                return Type.Renpy;

            if (line.StartsWith(")"))
                return Type.Closing;

            return Type.Talk;
        }
        private int GetIdent(string line) {
            var tempLine = line;
            int spaceCount = 0;

            while (tempLine.Count() > 0 && tempLine[0] == ' ') {
                spaceCount++;
                tempLine = tempLine.Substring(1);
            }

            int thisIdent = (spaceCount / _identSpaces);

            if ((spaceCount % _identSpaces) > (_identSpaces / 2))
                thisIdent++;

            return thisIdent;
        }
        private string GetIdented(int identLevel) {
            if (identLevel <= 0)
                return "";

            return " ".Repeat(_identSpaces * identLevel);
        }
        private int TestSymbols(string thisLine, int startPos, params char[] symbols) {
            var symbolPos = -1;

            var newStr = thisLine.Substring(startPos);


            foreach (var s in symbols) {
                symbolPos = newStr.IndexOf(s);

                if (symbolPos > -1)
                    break;
            }

            return symbolPos + startPos;
        }
    }

    public enum Type {
        Talk,
        Variable,
        Renpy,
        RenpyFn,
        Menu,
        MenuItem,
        Send,
        Operator,
        Image,
        ImageExtender,
        Empty,
        Comment,
        DecompilerComment,
        Python,
        Closing
    }

    public static class StaticObj {
        public static string[] RemoveEmpty(this string[] obj) {
            if (obj == null || obj.Length == 0)
                return obj;

            var listObj = obj.ToList();

            for (var i = 0; i < listObj.Count; i++)
                if (listObj[i].Trim() == "") {
                    listObj.RemoveAt(i);
                    i--;
                }

            return listObj.ToArray();
        }

        public static string Repeat(this string This, int times) {
            var textRet = "";

            if (This == null || times < 2)
                return This;

            for (var i = 0; i < times; i++)
                textRet += This;

            return textRet;
        }

        public static bool In(this Type This, params Type[] obj) {
            foreach (var o in obj)
                if (This == o)
                    return true;

            return false;

        }

        public static bool In(this char This, params char[] obj) {
            foreach (var o in obj)
                if (This == o)
                    return true;

            return false;

        }

        public static bool StartsWithAny(this string This, params string[] paramArray) {
            foreach (var p in paramArray)
                if (This.StartsWith(p, StringComparison.OrdinalIgnoreCase))
                    return true;

            return false;
        }
    }
}
