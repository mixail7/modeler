using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testcoco
{
    public enum Connecor
    {
        Input,
        Output,
        Error,
        PredCond,
        PostCond
    }

    public class Link
    {
        public string From;
        public string To;
        public string LinkNote;
        public Connecor FromConnector;
        public Connecor ToConnector;
    }

    public class Statment
    {
        public Token Tok;
        public Token IfTok;
        public Token ElseTok;
        public List<Token> Prop = new List<Token>();
    }

    public class SemanticProc
    {
        public List<Statment> Statments = new List<Statment>();
        public string GraphName;

        public static string ConnToStr(Connecor conn)
        {
            if (conn == Connecor.Error)
                return "Error";
            if (conn == Connecor.Input)
                return "Input";
            if (conn == Connecor.Output)
                return "Output";
            if (conn == Connecor.PostCond)
                return "PostCond";
            if (conn == Connecor.PredCond)
                return "PredCond";
            return conn.ToString();
        }

        public Token OptTok(Token tn)
        {
            if (tn == null)
            {
                return null;
            }
            if (tn.val.Length >= 2)
                tn.val = tn.val.Substring(1, tn.val.Length - 2);
            return tn;
        }

       public void SetGraphName(string graphName)
        {
            GraphName = graphName;
        }

        public void AddStatment(Statment st)
        {
            Statments.Add(st);
        }
        
        private int getCountProp(int stat_id, int prop_id)
        {
            int ct = 0;
            bool isStep = false;
            List<Token> props = Statments[stat_id].Prop;
            for (int i = prop_id; i >= 0; i--)
            {
                if (props[prop_id].col == props[i].col)
                {
                    ct++;
                }
                else if (props[prop_id].col > props[i].col)
                {
                    break;
                }
                else
                {
                    isStep = true;
                }
            }
            if (isStep && ct == 1)
            {
                return -1;
            }
            return ct;
        }
        
        public List<Link> GetLinks()
        {
            var list = new List<Link>();
            for (int i = 0; i < Statments.Count; i++)
            {
                Statment curren = Statments[i], next = i >= Statments.Count - 1 ? null : Statments[i + 1];
                if (curren.Prop.Count != 0)
                {
                    Token root = curren.Tok, predToken = curren.Prop[0];
                    for (int j = 0; j < curren.Prop.Count; j++)
                    {
                        if (curren.Prop[j].col == predToken.col)
                        {
                            predToken = curren.Prop[j];
                        }
                        else if (curren.Prop[j].col > predToken.col)
                        {
                            root = predToken;
                            predToken = curren.Prop[j];
                        }
                        else if (curren.Prop[j].col < predToken.col)
                        {
                            bool isFind = false;
                            predToken = curren.Prop[j];
                            for (int k = j; k >=0; k--)
                            {
                                if (curren.Prop[k].col < curren.Prop[j].col)
                                {
                                    isFind = true;
                                    root = curren.Prop[k];
                                    break;
                                }
                            }
                            if (!isFind)
                            {
                                root = curren.Tok;
                            }
                        }

                        if (curren.Prop[j].val.Length != 0)
                        {
                            Link lk = new Link();
                            lk.From = root.val;
                            if (root.val.Length == 0)
                            {
                                Console.WriteLine("Empty root property in line {0} col {1}", root.line, root.col);
                            }
                            lk.To = curren.Prop[j].val;
                            lk.ToConnector = Connecor.Input;
                            int poz = getCountProp(i, j);
                            if (poz > 3)
                            {
                                Console.WriteLine("Error in line {0} col {1}, Many Properties.",
                                                  curren.Prop[j].line, curren.Prop[j].col);
                                return list;
                            }
                            if (poz == -1)
                            {
                                Console.WriteLine("No root element for property in line {0} col {1}",
                                                  curren.Prop[j].line, curren.Prop[j].col);
                                return list;
                            }
                            switch (poz)
                            {
                                case 1:
                                    lk.FromConnector = Connecor.Error;
                                    break;
                                case 2:
                                    lk.FromConnector = Connecor.PredCond;
                                    break;
                                case 3:
                                    lk.FromConnector = Connecor.PostCond;
                                    break;
                            }
                            list.Add(lk);
                        }
                    }
                }
                if (next != null && (curren.Tok.col == next.Tok.col  || curren.Tok.col < next.Tok.col))
                {
                    if (next.ElseTok == null)
                    {
                        var lk = new Link
                            {
                                From = curren.Tok.val,
                                FromConnector = Connecor.Output,
                                ToConnector = Connecor.Input,
                                To = next.Tok.val
                            };
                        if (curren.IfTok != null)
                        {
                            lk.LinkNote = curren.IfTok.val;
                        }
                        if (curren.ElseTok != null)
                            lk.LinkNote = curren.ElseTok.val;
                        list.Add(lk);
                    }
                    else
                    {
                        bool isProc = false;
                        for (int j = i + 1; j < Statments.Count; j++)
                        {
                            if (Statments[j].Tok.col < curren.Tok.col && Statments[j].ElseTok == null)
                            {
                                isProc = true;
                                var lk = new Link
                                {
                                    From = curren.Tok.val,
                                    FromConnector = Connecor.Output,
                                    ToConnector = Connecor.Input,
                                    To = Statments[j].Tok.val,
                                };
                                list.Add(lk);
                            }
                        }
                        if (isProc != true)
                            Console.WriteLine("Error cannot find end point in line {0} col {1}",
                                              curren.Tok.line, curren.Tok.col);
                    }
                }
                
                if (next != null && curren.Tok.col > next.Tok.col)
                {
                    if (curren.ElseTok != null)
                    {
                        bool isProc = false;
                        for (int j = i; j >= 0; j--)
                        {
                            if (Statments[j].Tok.col < curren.Tok.col && Statments[j].IfTok != null)
                            {
                                isProc = true;
                                var lk = new Link
                                {
                                    From = Statments[j].Tok.val,
                                    FromConnector = Connecor.Output,
                                    ToConnector = Connecor.Input,
                                    To = curren.Tok.val,
                                    LinkNote = "else"
                                };
                                list.Add(lk);
                            }
                        }
                        if (isProc != true)
                            Console.WriteLine("Error find if statment for else statment in line {0} col {1}",
                                              curren.Tok.line, curren.Tok.col);
                    }

                    if (next.ElseTok == null)
                    {
                        var lk = new Link
                        {
                            From = curren.Tok.val,
                            FromConnector = Connecor.Output,
                            ToConnector = Connecor.Input,
                            To = next.Tok.val
                        };
                        list.Add(lk);
                    }
                    else
                    {
                        int poz = 0;
                        for (int j = i; j < Statments.Count; j++)
                        {
                            if (Statments[j].Tok.col == next.Tok.col - poz)
                            {
                                if (Statments[j].ElseTok != null)
                                {
                                    poz++;
                                    continue;
                                }
                            }
                            var lk = new Link
                            {
                                From = curren.Tok.val,
                                FromConnector = Connecor.Output,
                                ToConnector = Connecor.Input,
                                To = Statments[j].Tok.val
                            };
                            list.Add(lk);
                        }
                    }
                }
            }
            return list;
        }

        public static void PrintRes(List<Link> links)
        {
            for (int i = 0; i < links.Count; i++)
            {
                Link lk = links[i];
                Console.WriteLine("Link from '{0}' ({1}) to '{2}' ({3}), link note '{4}'", lk.From,
                                  ConnToStr(lk.FromConnector), lk.To, ConnToStr(lk.ToConnector), lk.LinkNote);

            }
        }
    }
}
