using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


enum TokenType{
    IDENTIFIER,
    KEYWORD,
    OPERATOR,
    DELIMITER,
    DECIMAL,
    FLOAT,
    STRING
}

public class Check_elements{
    public char ch;					                //一文字を格納
        public bool is_null(){
        return this.ch == '\0';                         //nullか?
    }
    public bool is_space(){                             //空白か?
        return this.ch == ' ';
    }
    public bool is_line_end(){                          //改行か?
        return this.ch == '\n';
    }
    public bool is_sharp(){
        return this.ch =='#';                           //シャープか?
    }
    public bool is_alpha(){                             //英字or_か?
        return (Regex.IsMatch(this.ch.ToString(), "[a-zA-Z]") || this.ch == '_');
    }
    public bool is_num(){                               //数字か?
        return Regex.IsMatch(this.ch.ToString(), "[0-9]");
    }
    public bool is_left_Sbracket(){                     //左カッコか?
        return this.ch == '(';
    }
    public bool is_right_Sbracket(){                    //右カッコか?
        return this.ch == ')';
    }
    public bool is_left_Mbracket(){                     //左中カッコか?
        return this.ch == '{';
    }
    public bool is_right_Mbracket(){                    //右中カッコか?
        return this.ch == '}';
    }
    public bool is_left_Lbracket(){                     //左大カッコか?
    	return this.ch == '[';
    }
    public bool is_right_Lbracket(){                    //右大カッコか?
    	return this.ch == ']';
    }
    public bool is_period(){                            //ピリオドか?
        return this.ch == '.';
    }
    public bool is_comma(){                             //コンマか?
        return this.ch == ',';
    }
    public bool is_colon(){                             //コロンか?
    	return this.ch == ':';
    }
    public bool is_semicolon(){                         //セミコロンか?
        return this.ch == ';';
    }
    public bool is_equal(){                             //イコールか?
        return this.ch == '=';
    }
    public bool is_plus(){                              //プラスか?
        return this.ch == '+';
    }
    public bool is_minus(){                             //マイナスか?
        return this.ch == '-';
    }
    public bool is_asterisk(){                          //アスタリスクか?
        return this.ch == '*';
    }
    public bool is_slash(){                             //スラッシュか?
        return this.ch == '/'; 
    }
    public bool is_back_slash(){                        //バックスラッシュか?
        return this.ch == '\\';
    }
    public bool is_percent(){                           //パーセントか?
    	return this.ch == '%';
    }
    public bool is_and(){                               //アンドか?
    	return this.ch == '&';
    }
    public bool is_or(){                                //オアか?
        return this.ch == '|';
    }
    public bool is_exclamation(){                       //ビックリか?
        return this.ch == '!';
    }
    public bool is_question(){                          //はてなか?
    	return this.ch == '?';
    }
    public bool is_less(){                              //小なりか?
    	return this.ch == '<';
    }
    public bool is_greater(){                           //大なりか?
    	return this.ch == '>';
    }
    public bool is_hat(){                               //ハットか?
    	return this.ch == '^';
    }
    public bool is_tilde(){                             //チルダか?
        return this.ch == '~';
    }
    public bool is_double_quot(){                       //ダブルクオーテーションか?
        return this.ch == '\"';
    }
    public bool is_single_quot(){                       //シングルクオーテーションか?
        return this.ch == '\'';
    }
}

public class Yoyakugo_check{
    public string id;			                        //識別子
    string[] type = new string[9]{ "int",
                                   "void",
                                   "char",
                                   "double",
                                   "float",
                                   "auto",
                                   "enum",
                                   "struct",
                                   "union"};

    string[] Adj_type = new string[4]{ "long",
                                       "signed",
                                       "unsigned",
                                       "static"};

    string[] yoyakugo = new string[19]{ "break",
	            			            "case",
                				        "const",
				                        "continue",
			                	        "default",
                				        "do",
                				        "else",
                				        "extern",
				                        "for",
				                        "goto",
				                        "if",
				                        "register",
			                	        "return",
			                	   	    "sizeof",
			                	   	    "short",
		                    		   	"switch",
			                   	       	"typedef",
	    		                	   	"volatile",
		    	                	   	"while"};

    public string yoyakugo_name(){
        foreach (string str in type){
            if (id == str) return "type";
            }
    	foreach (string str in Adj_type){
            if (id == str) return "Adj_type";
        }
        foreach (string str in yoyakugo){
            if (id == str) return "yoyakugo";
        }

        return "id";
    }
}

public class Node{
    public int num;     //自分の番号
    public string rule;  //生成規則
    public string token;    //ノードが持つトークン
    public string token_type;    //トークンの種類
    public int? left;  //子ノード(左)
    public int? right; //子ノード(右)
    public bool is_searched;    //探索済みか?
    public Node(){
        this.num = -1;
        this.rule = "None";
        this.token = "None";
        this.token_type = "None";
        this.left = null;
        this.right = null;
        this.is_searched = true;
    }
}

public static class Jikukaiseki{

    static public List<string> token = new List<string>();              //token一覧         Jikukaiseki.token[n]
    static public List<string> TokenTypeList = new List<string>();      //tokenの種類一覧   Jikukaiseki.TokenTypeList[n]
    static List<List<char>> tokenList = new List<List<char>>();

    public static void Jiku(){

        //StreamReader sr = new StreamReader(@"qsort.c");
        //string program = sr.ReadToEnd() + "\0";
        string program = "1*2-3+4-6*(7+8-9*10)" + "\0";

        Check_elements check = new Check_elements();
	    Yoyakugo_check yoyaku = new Yoyakugo_check();


        for (int i=0; i<program.Length; ++i){
            check.ch = program[i];               //programのi文字目をcheck.chに格納
            if (check.is_null()) break;
            if (check.is_space() || check.is_line_end()) continue;                  //空白or改行

        //コメントアウト
            else if (check.is_slash()){                     ///
                List<char> comment = new List<char>();
                comment.Add(check.ch);
                check.ch = program[++i];
                if (check.is_slash()){                      ////
                    comment.Add(check.ch);
                    while (!check.is_line_end()){
                        check.ch = program[++i];
                        comment.Add(check.ch);
                    }
                }
                else if (check.is_asterisk()){              ///*
                    comment.Add(check.ch);
                    while (true){
                        check.ch = program[++i];
                        comment.Add(check.ch);
                        if (check.is_asterisk()){
                            check.ch = program[++i];
                            if (check.is_slash()){          //*/
                                comment.Add(check.ch);
                                break;
                            }
                            comment.Add(check.ch);
                        }
                    }
                }else{
                    --i;
                    tokenList.Add(comment);
                    TokenTypeList.Add("factor_operator");
                    continue;
                }
            }

        //識別子、数字、後置インクリメント            
            else if (check.is_alpha()){                      //1文字目が[a-zA-Z]なら
                List<char> id = new List<char>();
                id.Add(check.ch);                       //idにcheck.chを格納
                check.ch = program[++i];         //check.chを1文字進める

                while (check.is_alpha() || check.is_num()){  //2文字目以降が[0-9a-z]or[A-Z]の間
                    id.Add(check.ch);                         //idに一文字追加
                    check.ch = program[++i];     //check.chを1文字進める
                }

                if (check.is_plus()){
                    check.ch = program[++i];     //check.chを1文字進める
                    if (check.is_plus()){
                        id.Add('+');
                        id.Add('+');
                        tokenList.Add(id);
                        TokenTypeList.Add("increment");
                        continue;
                    }
                    --i;                                    //check.chを1文字戻す
                }
                else if (check.is_minus()){
                    check.ch = program[++i];     //check.chを1文字進める
                    if (check.is_minus()){
                        id.Add('-');
                        id.Add('-');
                        tokenList.Add(id);
                        TokenTypeList.Add("decrement");
                        continue;
                    }
                    --i;                                    //check.chを1文字戻す
                }
                --i;                                         //check.chを1文字戻す
                tokenList.Add(id);
                string t = "";
                foreach(char ch in id){
                    t += ch;
                }
                yoyaku.id = t;
                t = yoyaku.yoyakugo_name();
                TokenTypeList.Add(t);
                continue;
            }

            else if (check.is_num() || check.is_period()){                       //1文字目が[0-9]なら
                List<char> num = new List<char>();
                bool period_flag = false;
                num.Add(check.ch);
                check.ch = program[++i];         //check.chを1文字進める

                while (check.is_num()){                      //2文字目以降が[0-9]の間
                    num.Add(check.ch);;                        //numに1文字追加
                    check.ch = program[++i];     //check.chを1文字進める
                }

                if (check.is_period()){
                    period_flag = true;
                    num.Add(check.ch);
                    check.ch = program[++i];
                    while (check.is_num()){
                        num.Add(check.ch);
                        check.ch = program[++i];
                    }
                }
                --i;                                        //check.chを1文字戻す
                tokenList.Add(num);
                if (period_flag==true) TokenTypeList.Add("float");
                if (period_flag==false) TokenTypeList.Add("decimal");
                continue;
            }

        //前置インクリメント
            else if (check.is_plus()){                      //+
                List<char> inc_plus = new List<char>();       //inc_plusにcheck.chを格納
                inc_plus.Add(check.ch);
                check.ch = program[++i];         //check.chを1文字進める

                if (check.is_plus()){                       //++
                    check.ch = program[++i];     //check.chを1文字進める

                    if (check.is_alpha()){                  //++[a-zA-Z]
                        inc_plus.Add(check.ch);
                        inc_plus.Add(check.ch);
                        check.ch = program[++i];
                        while (check.is_alpha() || check.is_num()){  //2文字目以降が[0-9a-z]or[A-Z]の間
                            inc_plus.Add(check.ch);                         //inc_plusに一文字追加            
                            check.ch = program[++i];     //check.chを1文字進める
                        }
                    }else{
                    --i;
                    tokenList.Add(inc_plus);
                    TokenTypeList.Add("increment");
                    continue;
                    }
                }
                --i;
                tokenList.Add(inc_plus);
                TokenTypeList.Add("term_operator");
                continue;
            }

            else if (check.is_minus()){                      //-
                List<char> inc_minus = new List<char>();
                inc_minus.Add(check.ch);                 //inc_minusにcheck.chを格納
                check.ch = program[++i];         //check.chを1文字進める

                if (check.is_minus()){                       //--
                    check.ch = program[++i];     //check.chを1文字進める

                    if (check.is_alpha()){                  //--[a-zA-Z]
                        inc_minus.Add(check.ch);
                        inc_minus.Add(check.ch);
                        check.ch = program[++i];
                        while (check.is_alpha() || check.is_num()){  //2文字目以降が[0-9a-z]or[A-Z]の間
                            inc_minus.Add(check.ch);                         //inc_minusに一文字追加            
                            check.ch = program[++i];            //check.chを1文字進める
                        }
                    }else{
                    --i;
                    tokenList.Add(inc_minus);
                    TokenTypeList.Add("decrement");
                    continue;
                    }
                }
                --i;
                tokenList.Add(inc_minus);
                TokenTypeList.Add("term_operator");
                continue;
            }

        //演算子
            else if (check.is_less()){                                //<
                List<char> less = new List<char>();
                less.Add(check.ch);
                check.ch = program[++i];
                if(check.is_equal()){                                 //<=
                    less.Add(check.ch);
                    ++i;
                }
                else if (check.is_less()){                            //<<
                    less.Add(check.ch);
                    tokenList.Add(less);
                    TokenTypeList.Add("bit_operator");
                    continue;
                }
                --i;
                tokenList.Add(less);
                TokenTypeList.Add("compare_operator");
                continue;
            }

            else if (check.is_greater()){                               //>
                List<char> greater = new List<char>();
                greater.Add(check.ch);
                check.ch = program[++i];
                if(check.is_equal()){                                 //>=
                    greater.Add(check.ch);
                    ++i;
                }
                else if (check.is_greater()){                            //>>
                    greater.Add(check.ch);
                    tokenList.Add(greater);
                    TokenTypeList.Add("bit_operator");
                    continue;
                }
                --i;
                tokenList.Add(greater);
                TokenTypeList.Add("compare_operator");
                continue;
            }

            else if (check.is_equal()){                                //=
                List<char> equal = new List<char>();
                equal.Add(check.ch);
                check.ch = program[++i];
                if(check.is_equal()){                                 //==
                    equal.Add(check.ch);
                    ++i;
                }
                --i;
                tokenList.Add(equal);
                TokenTypeList.Add("equal");
                continue;
            }

            else if (check.is_exclamation()){                     //!
                List<char> not_equal = new List<char>();
                not_equal.Add(check.ch);
                check.ch = program[++i];
                if(check.is_equal()){                                 //!=
                    not_equal.Add(check.ch);
                    tokenList.Add(not_equal);
                    TokenTypeList.Add("compare_operator");
                    continue;
                }
                --i;
                tokenList.Add(not_equal);
                TokenTypeList.Add("logic_operator");
                continue;
            }

            else if (check.is_and()){                     //&
                List<char> and = new List<char>();
                and.Add(check.ch);
                check.ch = program[++i];
                if(check.is_and()){                                 //&&
                    and.Add(check.ch);
                    tokenList.Add(and);
                    TokenTypeList.Add("logic_operator");
                    continue;
                }
                --i;
                tokenList.Add(and);
                TokenTypeList.Add("bit_operator");
                continue;
            }

            else if (check.is_or()){                     //|
                List<char> or = new List<char>();
                or.Add(check.ch);
                check.ch = program[++i];
                if(check.is_or()){                                 //||
                    or.Add(check.ch);
                    tokenList.Add(or);
                    TokenTypeList.Add("logic_operator");
                    continue;
                }
                --i;
                tokenList.Add(or);
                TokenTypeList.Add("bit_operator");
                continue;
            }

        //ポインタ関連
           
            else if (check.is_minus()){                      //->
                List<char> arrow = new List<char>();
                arrow.Add(check.ch);
                check.ch = program[++i];
                if(check.is_greater()){
                    arrow.Add(check.ch);
                    tokenList.Add(arrow);
                    TokenTypeList.Add("pointer_operator");
                    continue;
                }
                --i;
                continue;
            }


        //文字列
            else if (check.is_double_quot()){
                List<char> str = new List<char>();
                str.Add(check.ch);
                check.ch = program[++i];
                while(!check.is_double_quot()){
                    str.Add(check.ch);
                    check.ch = program[++i];
                }
                    str.Add('"');
                    tokenList.Add(str);
                    TokenTypeList.Add("string");
            }

            else if (check.is_single_quot()){
                List<char> str = new List<char>();
                str.Add(check.ch);
                check.ch = program[++i];
                if (check.is_alpha() || check.is_num()){
                    str.Add(check.ch);
                    check.ch = program[++i];
                    str.Add(check.ch);
                }
                tokenList.Add(str);
                TokenTypeList.Add("string");
                continue;
            }

        //その他記号(1文字)
            else{
                List<char> kigou = new List<char>();
                kigou.Add(check.ch);
                tokenList.Add(kigou);
                if (check.is_left_Sbracket()) TokenTypeList.Add("left_Sbracket");
                else if (check.is_right_Sbracket()) TokenTypeList.Add("right_Sbracket");
                else if (check.is_left_Mbracket()) TokenTypeList.Add("left_Mbracket");
                else if (check.is_right_Mbracket()) TokenTypeList.Add("right_Mbracket");
                else if (check.is_left_Lbracket()) TokenTypeList.Add("left_Lbracket");
                else if (check.is_right_Lbracket()) TokenTypeList.Add("right_Lbracket");
                else if (check.is_plus()) TokenTypeList.Add("term_operator");
                else if (check.is_minus()) TokenTypeList.Add("term_operator");
                else if (check.is_percent()) TokenTypeList.Add("factor_operator");
                else if (check.is_asterisk()) TokenTypeList.Add("factor_operator");
                else if (check.is_slash()) TokenTypeList.Add("factor_operator");
                else if (check.is_equal()) TokenTypeList.Add("equal");
                else if (check.is_comma()) TokenTypeList.Add("comma");
                else if (check.is_period()) TokenTypeList.Add("period");
                else if (check.is_colon()) TokenTypeList.Add("colon");
                else if (check.is_semicolon()) TokenTypeList.Add("semicolon");
                else if (check.is_sharp()) TokenTypeList.Add("sharp");
                else if (check.is_tilde()) TokenTypeList.Add("bit_operator");
                else if (check.is_hat()) TokenTypeList.Add("bit_operator");
                else TokenTypeList.Add("Not defined");
                continue;
            }
        }

        foreach (List<char> str in tokenList)
        {
            string token_str = "";
            foreach (char ch in str){
                token_str += ch;
            }
            token.Add(token_str);
        }
        for (int j=0; j<TokenTypeList.Count; ++j){
            Console.Write(token[j] + ", ");
            Console.WriteLine(TokenTypeList[j]);
        }
        token.Add("\0");
        TokenTypeList.Add("\0");
    }
}

public static class Koubunkaiseki{

    //木のすべてのノード　token*2の個数分の大きさを持つ
    public static Node[] node = new Node[Jikukaiseki.token.Count*2];
    public const int FALSE = -1;
    public static int p = -1, count = -1, index;

    public static void MakeTree(int num, string rule, string token, string token_type){
        node[num].num = num;
        node[num].rule = rule;
        node[num].token = token;
        node[num].token_type = token_type;
        node[num].is_searched = false;
    }
    public static void MakeTree(int num, string rule, string token, string token_type, int? left){
        node[num].num = num;
        node[num].rule = rule;
        node[num].token = token;
        node[num].token_type = token_type;
        node[num].left = left;
        node[num].is_searched = false;
    }
    public static void MakeTree(int num, string rule, string token, string token_type, int? left, int? right){
        node[num].num = num;
        node[num].rule = rule;
        node[num].token = token;
        node[num].token_type = token_type;
        node[num].left = left;
        node[num].right = right;
        node[num].is_searched = false;
    }

    //生成規則
    //
    //Root ::= S
    //S ::= V=E;|V=V;
    //E ::= TE'
    //E'::= +TE'|-TE'|ep
    //T ::= FT'
    //T'::= *FT'|/FT'|ep
    //F ::= (E)|decimal
    //V ::= id
    //
    //Director(Root, S)=First(S)={id}
    //Director(S, V=E)=First(V=E)={id}
    //Director(S, V=V)=First(V=V)={id}
    //Director(E, TE')=First(TE')={decimal}
    //Director(E', +TE')=First(+TE')={+}
    //Director(E', -TE')=First(-TE')={-}
    //Director(E', ep)=Follow(E')={}
    //Director(T, FT')=First(FT')={decimal}
    //Director(T', *FT')=First(*FT)={*}
    //Director(T', /FT')=First(/FT)={/}
    //Director(T', ep)=Follow(T')={}
    //Director(F, (E))=First((E))={(}
    //Director(F, decimal)=First(decimal)={decimal}
    //Director(V, id)=First(id)={id}

    static string[] Director_E0 = new string[]{"decimal"};
    static string[] Director_Ep0 = new string[]{"term_operator"};
    static string[] Director_T0 = new string[]{"decimal"};
    static string[] Director_Tp0 = new string[]{"factor_operator"};
    static string[] Director_F0 = new string[]{"left_Sbracket"};
    static string[] Director_F1 = new string[]{"decimal"};
    static string[] Director_V0 = new string[]{"id"};


    //基本的にはLL(1)構文解析。Director集合に重なりがある生成規則は、再帰的下向き構文解析
    
    static int parseRoot(){   //根
    
        int prev = p, cp = count, a;

        if((a=parseE())!=FALSE){
            MakeTree(++count, "Root->S", "None", "None", a);
            return count;
        }
        p = prev;
        count = cp;
        return FALSE;
        
    }
    static int parseS(){
        int prev = p, cp = count, a, b, q;
        if ((a=parseV())!=FALSE &&
             Jikukaiseki.TokenTypeList[(q=++p)]=="equal" &&
            (b=parseE())!=FALSE &&
             Jikukaiseki.TokenTypeList[++p]=="semicolon"){
                MakeTree(++count, "S->V=E", Jikukaiseki.token[q], Jikukaiseki.TokenTypeList[q], a, b);
                return count;
        }
        p = prev;
        count = cp;
        if ((a=parseV())!=FALSE &&
             Jikukaiseki.TokenTypeList[(q=++p)]=="equal" &&
            (b=parseV())!=FALSE &&
             Jikukaiseki.TokenTypeList[++p]=="semicolon"){
                MakeTree(++count, "S->V=V", Jikukaiseki.token[q], Jikukaiseki.TokenTypeList[q], a, b);
                return count;
            }
        p = prev;
        count = cp;
        return FALSE;
    }

    static int parseE(){
        if(Director_E0.Any(value => value==Jikukaiseki.TokenTypeList[p+1])){
            int a = parseT();
            int b = parseEp();
            MakeTree(++count, "E->TE'","None", "None", a, b);
            return count; 
        }
        return FALSE;
    }
    static int parseEp(){
        if(Director_Ep0.Any(value => value==Jikukaiseki.TokenTypeList[p+1])){
            int q = ++p;    //term_operatorを捨てる
            int a = parseT();
            int b = parseEp();
            MakeTree(++count, "E'::= +TE'|-TE'|ep", Jikukaiseki.token[q], Jikukaiseki.TokenTypeList[q], a, b);
            return count;
        }
        return count;
    }
    static int parseT(){
        if(Director_T0.Any(value => value==Jikukaiseki.TokenTypeList[p+1])){
            int a = parseF();
            int b = parseTp();
            MakeTree(++count, "T->FT'", "None", "None", a, b);
            return count; 
        }
        return FALSE;
    }
    static int parseTp(){
        if(Director_Tp0.Any(value => value==Jikukaiseki.TokenTypeList[p+1])){
            int q = ++p;    //factor_operatorを捨てる
            int a = parseF();
            int b = parseTp();
            MakeTree(++count, "T'::= *FT'|/FT'|ep", Jikukaiseki.token[q], Jikukaiseki.TokenTypeList[q], a, b);
            return count;
        }
        return count;
    }
    static int parseF(){
        if(Director_F0.Any(value => value==Jikukaiseki.TokenTypeList[p+1])){
            ++p;    //(を捨てる
            int a = parseE();
            ++p;    //)を捨てる
            MakeTree(++count, "F->(E)", "None", "None", a);
            return count;
        }
        if(Director_F1.Any(value => value==Jikukaiseki.TokenTypeList[p+1])){
            int q = ++p;    //decimalを捨てる
            MakeTree(++count, "F->decimal", Jikukaiseki.token[q], Jikukaiseki.TokenTypeList[q]);
            return count;
        }
        return FALSE;
    }
    static int parseV(){
        if(Director_V0.Any(value => value==Jikukaiseki.TokenTypeList[p+1])){
            int q = ++p;    //idを捨てる
            MakeTree(++count, "V->id", Jikukaiseki.token[q], Jikukaiseki.TokenTypeList[q]);
            return count;
        }
        return FALSE;
    }

    public static void Koubun(){
        //nodeの初期化
        for(int num=0; num<node.Length; ++num) node[num] = new Node();
        
        int error = parseRoot();
        
        for (int n = 0; n<node.Length; ++n){
            if(node[n].num == -1){
                index = n;
                break;
            }
            if(error == FALSE){
                Console.WriteLine("Syntax error");
                break;
            }
            
            Console.Write(node[n].num+": ");
            //Console.Write(node[n].rule+", ");
            Console.Write("\""+node[n].token+"\", ");
            Console.Write(node[n].left+", ");
            Console.WriteLine(node[n].right);
            
        }
    }
}

public static class Calculate{   
    public static Stack stack = new Stack();
    static string[] operators = new string[]{"+","-","*","/","%"};
    public static void Calc(){
        for(int i=0; i<Koubunkaiseki.node.Length; ++i){
            if(Koubunkaiseki.node[i].token=="None") continue;
            if(operators.Any(value => value==Koubunkaiseki.node[i].token)){
                var a = int.Parse((string)stack.Pop());
                var b = int.Parse((string)stack.Pop());
                Console.Write(b);
                Console.Write(Koubunkaiseki.node[i].token);
                Console.WriteLine(a);

                switch(Koubunkaiseki.node[i].token){
                    case "+":
                        stack.Push((b+a).ToString());
                        break;
                    case "-":
                        stack.Push((b-a).ToString());
                        break;
                    case "*":
                        stack.Push((b*a).ToString());
                        break;
                    case "/":
                        stack.Push((b/a).ToString());
                        break;
                    case "%":
                        stack.Push((b%a).ToString());
                        break;
                }
                continue;
            }
            stack.Push(Koubunkaiseki.node[i].token);
        }


    }
}

public class MyMain{
    public static void Main(){
        Jikukaiseki.Jiku();
        Koubunkaiseki.Koubun();
        Calculate.Calc();
    }
}
