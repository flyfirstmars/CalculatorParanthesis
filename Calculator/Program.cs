//=====================================================================
//  File:      Calculator.cs
//
//  Summary:   Simple calculator
//
//  ---------------------------------------------------------------------
//  This file is part of the Microsoft .NET Framework SDK Code Samples.
//
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//
//  This source code is intended only as a supplement to Microsoft
//  Development Tools and/or on-line documentation.  See these other
//  materials for detailed information regarding Microsoft code samples.
//
//  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//  PARTICULAR PURPOSE.
//=====================================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Samples.CompactFramework;

namespace Microsoft.Samples.CompactFramework
{
    // Calculator window
    public class Window : Form
    {
        private Button[] buttons = new Button[(int) Command.Max];
        private int buttonCount;
        private Button capturedButton;

        private string[] buttonCaptions =
        {
            "M+",	"MR",	"MC",	"1/x",	"/",
            "+/-",	"7",	"8",	"9",	"x",
            "%",	"4",	"5",	"6",	"-",
            "CE",	"1",	"2",	"3",	"+",
            "C",	"0",	".",	"=",    "(",
            ")",
        };

        private Color[] buttonColors =
        {
            Color.DarkRed,  Color.DarkRed,  Color.DarkRed,  Color.DarkBlue,
            Color.DarkRed,  Color.DarkBlue, Color.DarkBlue, Color.DarkBlue,
            Color.DarkBlue, Color.DarkRed,  Color.DarkBlue, Color.DarkBlue,
            Color.DarkBlue, Color.DarkBlue, Color.DarkRed,  Color.DarkRed,
            Color.DarkBlue, Color.DarkBlue, Color.DarkBlue, Color.DarkRed,
            Color.DarkRed,  Color.DarkBlue, Color.DarkBlue, Color.DarkRed,
            Color.DarkRed,  Color.DarkRed
        };

        public enum Command
        {
            MemorySet = 0,
            MemoryRecall,
            MemoryClear,
            OneOver,
            Div,
            Minus,
            Seven,
            Eight,
            Nine,
            Multiply,
            Percent,
            Four,
            Five,
            Six,
            Sub,
            ClearEntry,
            One,
            Two,
            Three,
            Add,
            ClearAll,
            Zero,
            Dot,
            Equal,
            RightParenthesis,
            LeftParenthesis,
            Max,
        }

        private Edit editBox;
        private Calculator calc;
        private Font windowFont;

        public Window()
        {
            int x, y;
            int row, col;

            calc = new Calculator();

            var windowWidth = 240;
            var windowHeight = 340 - 20;

            windowFont = new Font(
                FontFamily.GenericSansSerif,
                12,
                FontStyle.Bold);

            ClientSize = new Size(windowWidth, windowHeight);
            MaximizeBox = false;

            // Calculate button size based on window size (button matrix is 5x5)

            var buttonWidth = 43;
            var buttonHeight = 46;

            var sizeMargin = Math.Min(buttonWidth / 8, buttonHeight / 8);

            buttonWidth -= sizeMargin * 2;
            buttonHeight -= sizeMargin * 2;

            // Calculate edit size

            var editX = sizeMargin;
            var editY = sizeMargin;
            var editWidth = windowWidth - (sizeMargin * 2);
            var editHeight = buttonHeight;

            // Create buttons

            var buttonTopRow = sizeMargin + editHeight + sizeMargin;

            buttons = new Button[(int) Command.Max];

            y = buttonTopRow;

            for (row = 0; row < 6; row++)
            {
                x = sizeMargin;

                for (col = 0; col < 5; col++)
                {
                    if (buttonCount < (int) Command.Max)
                    {
                        buttons[buttonCount] = new Button
                        (
                            this,
                            windowFont,
                            x,
                            y,
                            buttonWidth,
                            buttonHeight,
                            sizeMargin,
                            buttonCaptions[buttonCount],
                            buttonColors[buttonCount],
                            (Command) buttonCount
                        );

                        buttonCount++;
                    }

                    x += sizeMargin + buttonWidth + sizeMargin;
                }

                y += sizeMargin + buttonHeight + sizeMargin;
            }

            //useless code creates problems with layout
            // Adjust + button

            // buttons[(int) Command.Add].IsTall = true;

            // Create edit

            editBox = new Edit(this, windowFont, new Rectangle(editX, editY, editWidth, editHeight))
            {
                EditString = calc.Render()
            };

            BackColor = Color.SlateGray;
            Text = @"Calculator";
        }

        public sealed override Color BackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }

        public sealed override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        ~Window()
        {
            windowFont.Dispose();
        }

        private void DoCommand(Command cmd)
        {
            switch (cmd)
            {
                case Command.MemorySet:
                    calc.DoMemorySet();
                    break;

                case Command.MemoryClear:
                    calc.DoMemoryClear();
                    break;

                case Command.MemoryRecall:
                    calc.DoMemoryRecall();
                    break;

                case Command.ClearAll:
                    calc.DoClearAll();
                    break;

                case Command.ClearEntry:
                    calc.DoClearCurrentToken();
                    break;

                case Command.Percent:
                    calc.DoPercent();
                    break;

                case Command.OneOver:
                    calc.DoOneOver();
                    break;

                case Command.Sub:
                    calc.DoOperator(Token.TokenType.Subtract);
                    break;

                case Command.Add:
                    calc.DoOperator(Token.TokenType.Add);
                    break;

                case Command.Div:
                    calc.DoOperator(Token.TokenType.Divide);
                    break;

                case Command.Multiply:
                    calc.DoOperator(Token.TokenType.Multiply);
                    break;

                case Command.Minus:
                    calc.DoNegative();
                    break;

                case Command.Dot:
                    calc.DoDecimal();
                    break;

                case Command.Zero:
                    calc.DoDigit(0);
                    break;

                case Command.One:
                    calc.DoDigit(1);
                    break;

                case Command.Two:
                    calc.DoDigit(2);
                    break;

                case Command.Three:
                    calc.DoDigit(3);
                    break;

                case Command.Four:
                    calc.DoDigit(4);
                    break;

                case Command.Five:
                    calc.DoDigit(5);
                    break;

                case Command.Six:
                    calc.DoDigit(6);
                    break;

                case Command.Seven:
                    calc.DoDigit(7);
                    break;

                case Command.Eight:
                    calc.DoDigit(8);
                    break;

                case Command.Nine:
                    calc.DoDigit(9);
                    break;

                case Command.Equal:
                    calc.DoEvaluate();
                    break;

                case Command.LeftParenthesis:
                    calc.DoParenthesis(Token.TokenType.LeftParenthesis);
                    break;

                case Command.RightParenthesis:
                    calc.DoParenthesis(Token.TokenType.RightParenthesis);
                    break;
            }

            editBox.EditString = calc.Render();
        }

        protected override void OnPaint(PaintEventArgs paintArgs)
        {
            var graphics = paintArgs.Graphics;

            // Edit line

            editBox.Render(graphics);

            // Buttons

            foreach (var button in buttons)
            {
                button.Render(graphics);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs paintArgs)
        {
            base.OnPaintBackground(paintArgs);
        }

        protected override void OnMouseDown(MouseEventArgs mouseArgs)
        {
            foreach (var button in buttons)
            {
                if (!button.IsHit(mouseArgs.X, mouseArgs.Y)) continue;
                button.IsSelected = true;
                capturedButton = button;

                break;
            }
        }

        protected override void OnMouseMove(MouseEventArgs mouseArgs)
        {
            if (capturedButton != null)
            {
                capturedButton.IsSelected = capturedButton.IsHit(mouseArgs.X, mouseArgs.Y);
            }
        }

        protected override void OnMouseUp(MouseEventArgs mouseArgs)
        {
            if (capturedButton != null)
            {
                if (capturedButton.IsHit(mouseArgs.X, mouseArgs.Y))
                    DoCommand(capturedButton.Cmd);

                capturedButton.IsSelected = false;
                capturedButton = null;
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs keyArgs)
        {
            switch (keyArgs.KeyChar)
            {
                case '1':
                    DoCommand(Command.One);
                    break;

                case '2':
                    DoCommand(Command.Two);
                    break;

                case '3':
                    DoCommand(Command.Three);
                    break;

                case '4':
                    DoCommand(Command.Four);
                    break;

                case '5':
                    DoCommand(Command.Five);
                    break;

                case '6':
                    DoCommand(Command.Six);
                    break;

                case '7':
                    DoCommand(Command.Seven);
                    break;

                case '8':
                    DoCommand(Command.Eight);
                    break;

                case '9':
                    DoCommand(Command.Nine);
                    break;

                case '0':
                    DoCommand(Command.Zero);
                    break;

                case (char) (int) Keys.Back:
                    DoCommand(Command.ClearEntry);
                    break;

                case '.':
                    DoCommand(Command.Dot);
                    break;

                case '+':
                    DoCommand(Command.Add);
                    break;

                case '-':
                    DoCommand(Command.Sub);
                    break;

                case '*':
                    DoCommand(Command.Multiply);
                    break;

                case '/':
                    DoCommand(Command.Div);
                    break;

                case '=':
                case (char) 13:
                    DoCommand(Command.Equal);
                    break;

                case ')':
                    DoCommand(Command.LeftParenthesis);
                    break;
                case '(':
                    DoCommand(Command.RightParenthesis);
                    break;
            }
        }

        public static int Main()
        {
            var window = new Window();
            Application.Run(window);

            return 0;
        }

        // Code Below is never used, which is WEIRD 

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // Window
            // 
            ClientSize = new Size(292, 266);
            Name = "Window";
            Load += Window_Load;
            ResumeLayout(false);
        }

        private void Window_Load(object sender, EventArgs e)
        {
        }
    }
    // 
    // The following structure describes a number and the operations 
    // that can be performed on it. Its value is stored internally 
    // as a double.
    //
    public struct Number
    {
        readonly double numValue;

        public Number(double n)
        {
            numValue = n;
        }

        public static Number Add(Number a, Number b)
        {
            return new Number(a.numValue + b.numValue);
        }

        public static Number Subtract(Number a, Number b)
        {
            return new Number(a.numValue - b.numValue);
        }

        // public static Number LeftParanthesis(Number a, Number b)
        // {
        //     return (new Number((a.numValue)));
        // }
        //
        // public static Number RightParanthesis(Number a, Number b)
        // {
        //     return (new Number((b.numValue)));
        // }

        public static Number Multiply(Number a, Number b)
        {
            return new Number(a.numValue * b.numValue);
        }

        public static Number Divide(Number a, Number b)
        {
            if (b.numValue == 0)
                return new Number(0);
            return new Number(a.numValue / b.numValue);
        }

        public static bool operator ==(Number a, Number b)
        {
            return a.numValue.Equals(b.numValue);
        }

        public static bool operator !=(Number a, Number b)
        {
            return !a.numValue.Equals(b.numValue);
        }

        public override bool Equals(object b)
        {
            return b is Number number && numValue.Equals(number.numValue);
        }

        public override int GetHashCode()
        {
            return (int) numValue;
        }

        public override string ToString()
        {
            return numValue.ToString();
        }
    }
    
    // 
    // The following class implements a button control for the calculator
    //
public class Button
    {
        private Form MainForm;
        private int PositionLeftX;
        private int PositionTopY;
        private int SizeWidth;
        private int SizeHeight;
        private int SizeMargin;
        private string CaptionName;
        private Color CaptionColor;
        private Font CaptionFont;
        private bool IsTallValue;
        private bool IsSelectedValue;

        public Button(Form form, Font font, int x, int y, int width, int height,
            int margin, string capString, Color capColor, Window.Command cmd)
        {
            MainForm = form;
            CaptionFont = font;
            PositionLeftX = x;
            PositionTopY = y;
            SizeWidth = width;
            SizeHeight = height;
            SizeMargin = margin;
            CaptionName = capString;
            CaptionColor = capColor;
            Cmd = cmd;
        }

        public void Render(Graphics graphics)
        {
            Brush brush = new SolidBrush(IsSelectedValue ? CaptionColor : Color.White);
            var pen = new Pen(Color.Black);

            graphics.FillEllipse(brush, PositionLeftX, PositionTopY,
                SizeWidth, SizeHeight);
            graphics.DrawEllipse(pen, PositionLeftX, PositionTopY,
                SizeWidth, SizeHeight);

            var textWidth = (int) graphics.MeasureString(CaptionName,
                CaptionFont).Width;
            var textHeight = (int) graphics.MeasureString(CaptionName,
                CaptionFont).Height;

            var x = PositionLeftX + (SizeWidth - textWidth) / 2;
            var y = PositionTopY + (SizeHeight - textHeight) / 2;
            graphics.DrawString(CaptionName, CaptionFont,
                new SolidBrush(IsSelectedValue ? Color.White : CaptionColor),
                x, y);
            brush.Dispose();
        }

        public bool IsHit(int x, int y)
        {
            return x >= PositionLeftX &&
                   x < PositionLeftX + SizeWidth &&
                   y >= PositionTopY &&
                   y < PositionTopY + SizeHeight;
        }

        public bool IsTall
        {
            get => IsTallValue;
            set
            {
                IsTallValue = value;
                if (value) SizeHeight = SizeHeight * 2 + SizeMargin * 2;
            }
        }

        public bool IsSelected
        {
            get => IsSelectedValue;
            set
            {
                if (value != IsSelectedValue)
                {
                    IsSelectedValue = value;

                    // Redraw right away
                    var graphics = MainForm.CreateGraphics();
                    Render(graphics);
                    graphics.Dispose();
                }
            }
        }

        public Window.Command Cmd { get; }
    }
    //
    // The following class implements an Edit control.
    //
    public class Edit
    {
        private Form MainForm;
        private Rectangle AreaBounds;
        private string EditStringValue;
        private Font EditFont;

        public Edit(Form form, Font font, Rectangle rcBounds)
        {
            MainForm = form;
            EditFont = font;
            AreaBounds = rcBounds;
        }

        public void Render(Graphics graphics)
        {
            var brush = new SolidBrush(Color.Black) {Color = Color.White};

            graphics.FillRectangle(brush, AreaBounds);
            graphics.DrawRectangle(new Pen(Color.Black), AreaBounds);

            var str = EditStringValue;
            var textWidth = (int) graphics.MeasureString(str, EditFont).Width;
            var textHeight = (int) graphics.MeasureString(str, EditFont).Height;

            var x = AreaBounds.Left + AreaBounds.Width - textWidth;
            var y = AreaBounds.Top + (AreaBounds.Height - textHeight) / 2;

            graphics.Clip = new Region(AreaBounds);
            brush.Color = Color.Black;
            graphics.DrawString(str, EditFont, brush,
                x, y);
            graphics.ResetClip();
            brush.Dispose();
        }

        public string EditString
        {
            get => EditStringValue;

            set
            {
                EditStringValue = value;

                // Redraw right away
                var graphics = MainForm.CreateGraphics();
                Render(graphics);
                graphics.Dispose();
            }
        }
    }

    // The following class describes a mathematical operation token.
    // 
    //
     public class Token
    {
        private TokenType TypeValue;
        private Number TokenNumberValue;
        private int DecimalFactorValue;
        private bool IsSealedValue;
        static private char[] Symbols = {'+', '-', 'x', '/', ')', '('};

        public enum TokenType
        {
            Nil = -1,
            Add = 0,
            Subtract,
            Multiply,
            Divide,
            LeftParenthesis,
            RightParenthesis,
            TokenNumber
        }

        public Token(TokenType type)
        {
            DecimalFactor = 0;
            Type = type;
        }

        public TokenType Type
        {
            get { return (TypeValue); }
            set { TypeValue = value; }
        }

        public Number TokenNumber
        {
            get { return (TokenNumberValue); }
            set { TokenNumberValue = value; }
        }

        public int DecimalFactor
        {
            get { return (DecimalFactorValue); }
            set { DecimalFactorValue = value; }
        }

        public bool IsSealed
        {
            get { return (IsSealedValue); }

            set { IsSealedValue = value; }
        }

        public bool IsOperator()
        {
            return (Type >= TokenType.Add &&
                    Type <= TokenType.Divide);
        }
        
        public bool IsNumber()
        {
            return (Type == TokenType.TokenNumber);
        }

        public bool IsLessThanOrEqualTo(Token tokenCompare)
        {
            return (Type <= tokenCompare.Type);
        }

        public bool IsParenthesis() 
        {
            return (Type == TokenType.RightParenthesis || Type == TokenType.LeftParenthesis);
        }
        
        public override string ToString()
        {
            string resultString;

            if (IsOperator())

                resultString = new string(Symbols[(int) Type], 1);

            else if (IsParenthesis())
            {
                resultString = new string(Symbols[(int) Type], 1);
            }
            else
                resultString = TokenNumberValue.ToString();

            return (resultString);
        }
    }

     // Todo Add description for class Calculator
     //
    public class Calculator
    {
        private List<Token> TokenList = new List<Token>();
        private Token MemoryToken = new Token(Token.TokenType.TokenNumber);

        public Calculator()
        {
            Reset();
        }
        
        // public void DoRightParanthesis()
        // {
        //     throw new NotImplementedException();
        // }

        // public void DoLeftParanthesis()
        // {
        //     throw new NotImplementedException();
        // }

        public void DoParenthesis(Token.TokenType type)
        {
            if (CurrentToken().TokenNumber == new Number(0) && CurrentToken().IsNumber())
            {
                RemoveCurrentToken();
            }

            AddOperatorToken(type);
        }
        private void Reset()
        {
            TokenList.Clear();
            AddNumberToken(new Number(0));
        }

        private void AddNumberToken(Number number)
        {
            var tok = new Token(Token.TokenType.TokenNumber) {TokenNumber = number};
            TokenList.Add(tok);
        }

        private void AddOperatorToken(Token.TokenType type)
        {
            var tok = new Token(type);
            TokenList.Add(tok);
        }

        private void RemoveCurrentToken()
        {
            if (TokenList != null && TokenList.Count > 0)
            {
                TokenList.RemoveAt(TokenList.Count - 1);
            }
        }


        private Token FetchToken()
        {
            if (TokenList != null && TokenList.Count > 0)
            {
                Token tok;
                tok = TokenList[0];
                TokenList.RemoveAt(0);
                return (tok);
            }

            return (new Token(Token.TokenType.Nil));
        }


        private Token CurrentToken()
        {
            if (TokenList != null && TokenList.Count > 0)
                return (TokenList[TokenList.Count - 1]);
            return (new Token(Token.TokenType.Nil));
        }

        public void DoMemorySet()
        {
            if (CurrentToken().IsNumber())
            {
                MemoryToken.TokenNumber = CurrentToken().TokenNumber;
            }
        }

        public void DoMemoryClear()
        {
            MemoryToken.Type = Token.TokenType.Nil;
        }

        public void DoMemoryRecall()
        {
            if (MemoryToken.IsNumber())
            {
                if (CurrentToken().IsNumber())
                    RemoveCurrentToken();

                AddNumberToken(MemoryToken.TokenNumber);
                CurrentToken().IsSealed = true;
            }
        }

        public void DoDecimal()
        {
            if (!CurrentToken().IsNumber())
                AddNumberToken(new Number(0));

            if (CurrentToken().DecimalFactor == 0)
                CurrentToken().DecimalFactor = 10;
        }

        public void DoDigit(int n)
        {
            Token tok;


            if (CurrentToken().Type == Token.TokenType.TokenNumber &&
                CurrentToken().IsSealed)
            {
                RemoveCurrentToken();
                AddNumberToken(new Number(0));
            }

            if (CurrentToken().Type == Token.TokenType.TokenNumber)
            {
                tok = CurrentToken();

                if (tok.DecimalFactor == 0)
                {
                    tok.TokenNumber = Number.Add(
                        Number.Multiply(
                            tok.TokenNumber,
                            new Number(10)
                        ),
                        new Number(n)
                    );
                }
                else
                {
                    tok.TokenNumber = Number.Add(
                        tok.TokenNumber,
                        Number.Divide(
                            new Number(n),
                            new Number(tok.DecimalFactor)
                        )
                    );
                    tok.DecimalFactor *= 10;
                }
            }
            else
            {
                AddNumberToken(new Number(n));
            }
        }

        public void DoOperator(Token.TokenType type)
        {
            if (CurrentToken().IsOperator())
            {
                RemoveCurrentToken();
            }

            else if (CurrentToken().IsNumber())
            {
                CurrentToken().IsSealed = true;
            }

            AddOperatorToken(type);
        }


        public void DoNegative()
        {
            if (CurrentToken().IsNumber())
            {
                CurrentToken().TokenNumber = Number.Multiply(
                    CurrentToken().TokenNumber, new Number(-1));
            }
        }

        public void DoOneOver()
        {
            if (CurrentToken().IsNumber())
            {
                if (CurrentToken().TokenNumber == new Number(0))
                    CurrentToken().TokenNumber = new Number(0);
                else
                    CurrentToken().TokenNumber = Number.Divide(
                        new Number(1), CurrentToken().TokenNumber);
            }
        }

        public void DoClearAll()
        {
            Reset();
        }

        public void DoClearCurrentToken()
        {
            if (CurrentToken().IsNumber())
            {
                if ((CurrentToken().TokenNumber == new Number(0)) &&
                    (TokenList.Count > 1))
                    RemoveCurrentToken();
                else
                {
                    RemoveCurrentToken();
                    AddNumberToken(new Number(0));
                }
            }
            else if (CurrentToken().IsOperator())
            {
                RemoveCurrentToken();
            }
        }

        public void DoPercent()
        {
            if (CurrentToken().IsNumber())
            {
                CurrentToken().TokenNumber = Number.Divide(
                    CurrentToken().TokenNumber, new Number(100));
            }
        }

        private static Token TokenEvalBinOp(Token tokOp, Token aToken, Token bToken)
        {
            Token result;

            result = new Token(Token.TokenType.TokenNumber);

            switch (tokOp.Type)
            {
                case Token.TokenType.Add:
                    result.TokenNumber = Number.Add(
                        aToken.TokenNumber, bToken.TokenNumber);
                    break;

                case Token.TokenType.Subtract:
                    result.TokenNumber = Number.Subtract(
                        aToken.TokenNumber, bToken.TokenNumber);
                    break;

                case Token.TokenType.Multiply:
                    result.TokenNumber = Number.Multiply(
                        aToken.TokenNumber, bToken.TokenNumber);
                    break;

                case Token.TokenType.Divide:
                    result.TokenNumber = Number.Divide(
                        aToken.TokenNumber, bToken.TokenNumber);
                    break;
            }

            return (result);
        }

        private static void DoBinaryEval(Stack<Token> operatorStack1, Stack<Token> numberStack)
        {
            Token topOperatorToken;
            Token aToken, bToken;

            topOperatorToken = operatorStack1.Pop();

            bToken = numberStack.Pop();
            aToken = numberStack.Pop();

            numberStack.Push(
                TokenEvalBinOp(topOperatorToken, aToken, bToken));
        }

        private static void DoBinaryEval2(Stack<Token> operatorStack1, Stack<Token> numberStack)
        {
            Token topOperatorToken;
            Token aToken, bToken;

            topOperatorToken = operatorStack1.Pop();

            aToken = numberStack.Pop();
            bToken = numberStack.Pop();

            numberStack.Push(
                TokenEvalBinOp(topOperatorToken, aToken, bToken));
        }

        public void DoEvaluate()
        {
            var operatorStack1 = new Stack<Token>();
            var numberStack1 = new Stack<Token>();
            var operatorStack2 = new Stack<Token>();
            var numberStack2 = new Stack<Token>();
            Token currentToken;
         
            if (CurrentToken().IsOperator())
            {
                RemoveCurrentToken();
            }

            // Eval

            while ((currentToken = FetchToken()).Type != Token.TokenType.Nil)
            {
                if (currentToken.IsNumber())
                {
                    numberStack1.Push(currentToken);
                }
                else if (currentToken.IsOperator())
                {
                    if (operatorStack1.Count > 0)
                    {
                        var topOperatorTok = operatorStack1.Peek();
                        if (topOperatorTok.IsParenthesis())
                        {
                            operatorStack1.Push(currentToken);
                            continue;
                        }

                        if (currentToken.IsLessThanOrEqualTo(topOperatorTok))
                        {
                            DoBinaryEval(operatorStack1, numberStack1);
                        }
                    }

                    operatorStack1.Push(currentToken);
                }

                else if (currentToken.IsParenthesis())
                {
                    if (currentToken.Type == Token.TokenType.LeftParenthesis)
                    {
                        while (operatorStack1.Peek().Type != Token.TokenType.RightParenthesis
                        )
                        {
                            DoBinaryEval(operatorStack1, numberStack1);
                        }

                        operatorStack1.Pop();
                    }
                    else
                    {
                        operatorStack1.Push(currentToken);
                    }
                }
            }

            if (numberStack1.Count == 1)
            {
                currentToken = numberStack1.Pop();
                Reset();
                RemoveCurrentToken();
                AddNumberToken(currentToken.TokenNumber);
                CurrentToken().IsSealed = true;
            }
            else
            {
                // Empty the stack
                while (operatorStack1.Count > 0)
                {
                    DoBinaryEval(operatorStack1, numberStack1);
                }

            
                while (operatorStack2.Count > 0)
                {
                    DoBinaryEval2(operatorStack2, numberStack2);
                }

                // Update token list

                currentToken = numberStack1.Pop();
                Reset();
                RemoveCurrentToken();
                AddNumberToken(currentToken.TokenNumber);
                CurrentToken().IsSealed = true;
                
            }
        }

        public string Render()
        {
            var resultString = TokenList.Aggregate("", (current, tok) => current + (" " + tok + " "));

            return (resultString);
        }
    }
}

