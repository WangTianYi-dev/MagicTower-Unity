/*
 * file: Parser.cs
 * author: DeamonHook
 * feature: 地图中所用到的语法解析器
 */

using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InnerParser;

namespace InnerParser
{
    /*
     BEGIN_OBJECT（{）
     END_OBJECT（}）
     BEGIN_ARRAY（[）
     END_ARRAY（]）
     NULL（null）
     NUMBER（数字）
     STRING（字符串）
     BOOLEAN（true/false）
     SEP_COLON（:）
     SEP_COMMA（,）
     END_DOCUMENT（表示JSON文档结束）
    */
    public enum TokenType
    {
        BEGIN_OBJECT,
        END_OBJECT,
        BEGIN_ARRAY,
        END_ARRAY,
        NULL,
        NUMBER,
        STRING,
        BOOLEAN,
        SEP_COLON,
        SEP_COMMA,
        END_DOCUMENT
    }

    public class Token
    {
        public TokenType tokenType;
        public string value;

        public Token(TokenType type, string value)
        {
            tokenType = type;
            this.value = value;
        }
    }

    public class StringStream
    {
        private string value;
        private int index;

        public StringStream(string val)
        {
            value = val;
            index = 0;
        }

        public bool HasNext()
        {
            return index < value.Length;
        }

        public char Next()
        {
            return value[index++];
        }
    }
}

public static class Parser
{
    private static List<Token> tokenList;

    private static StringStream stream;

    /// <summary>
    /// 将输入Token化
    /// </summary>
    /// <param name="rawData">原始数据</param>
    private static void Tokenizer(string rawData)
    {
        tokenList = new List<Token>();
        stream = new StringStream(rawData);
        Token t;
        do
        {
            t = NextToken();
            tokenList.Add(t);
        }
        while (t.tokenType != TokenType.END_DOCUMENT);
    }

    /// <summary>
    /// 获取流
    /// </summary>
    /// <returns></returns>
    private static Token NextToken() 
    {
        char ch;
        for (; ; )
        {
            if (!stream.HasNext())
            {
                return new Token(TokenType.END_DOCUMENT, null);
            }

            ch = stream.Next();

            if (!char.IsWhiteSpace(ch))
            {
                break;
            }
        }

        switch (ch)
        {
            case '{':
                return new Token(TokenType.BEGIN_OBJECT, ch.ToString());
            case '}':
                return new Token(TokenType.END_OBJECT, ch.ToString());
            case '[':
                return new Token(TokenType.BEGIN_ARRAY, ch.ToString());
            case ']':
                return new Token(TokenType.END_ARRAY, ch.ToString());
            case ',':
                return new Token(TokenType.SEP_COMMA, ch.ToString());
            case ':':
                return new Token(TokenType.SEP_COLON, ch.ToString());
            case 'n':
                return ReadNull();
            case 't':
            case 'f':
                return ReadBoolean();
            case '"':
                return ReadString();
            case '-':
                return ReadNumber();
        }

        if (char.IsDigit(ch))
        {
            return ReadNumber();
        }

        throw new Exception("Illegal character");
    }

    /// <summary>
    /// json所允许的escape character
    /// </summary>
    private readonly static HashSet<char> escapeSet = new HashSet<char> {
        'b', 'f', 'n', 'r', 't', 'u', '/', '\\'
    };

    private static Token ReadNull()
    {
        throw new NotImplementedException();
    }

    private static Token ReadBoolean()
    {
        throw new NotImplementedException();
    }

    private static Token ReadNumber()
    {
        throw new NotImplementedException();
    }

    private static Token ReadString()
    {
        throw new NotImplementedException();
        //StringBuilder sb = new StringBuilder();
        //for (; ; )
        //{
        //    char ch = stream.Next();
        //    if (ch == '\\')
        //    {
        //        ch = stream.Next();
        //        if (!escapeSet.Contains(ch))
        //        {
        //            throw new Exception("Illegal escape character");
        //        }
        //        else
        //        {
        //            sb.Append('\\');
        //            sb.Append(ch);
        //            if (ch == 'u')
        //            {
        //                for (int i = 0; i < 4; i++)
        //                {
        //                    ch = stream.Next();
                            
        //                }
        //            }
        //        }

        //    }

        //}
        
    }

    private static bool IsQuote(char c)
    {
        return c == '"' || c == '“' || c == '”';
    }

    /// <summary>
    /// 读取对话
    /// </summary>
    /// <param name="input">对话</param>
    /// <returns></returns>
    public static List<string> LoadDialog(string raw)
    {
        var ss = new StringStream(raw);
        // TODO 需要重写
        Func<string> nextString = () =>
        {
            var s = new StringBuilder();
            char ch;
            for (; ; )
            {
                ch = ss.Next();
                if (IsQuote(ch))
                {
                    break;
                }
                s.Append(ch);
                if (ch == '\\')
                {
                    s.Append(ss.Next());
                }
            }
            return s.ToString();
        };

        var lst = new List<string>();

        while (ss.HasNext())
        {

            char ch = ss.Next();
            if (IsQuote(ch))
            {
                lst.Add(nextString());
            }
        }

        return lst;
    }
}
