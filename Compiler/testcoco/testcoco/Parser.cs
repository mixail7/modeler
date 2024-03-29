using testcoco;



using System;



public class Parser {
	public const int _EOF = 0;
	public const int _ident = 1;
	public const int _cond = 2;
	public const int _eol = 3;
	public const int maxT = 8;

	const bool T = true;
	const bool x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

public SemanticProc Sem = new SemanticProc();


	public Parser(Scanner scanner) {
		this.scanner = scanner;
		errors = new Errors();
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}

	
	void Sample() {
		Expect(4);
		GraphName();
		Expect(3);
		while (la.kind == 3) {
			Get();
		}
		while (la.kind == 1) {
			Statment();
		}
		Expect(5);
	}

	void GraphName() {
		Expect(1);
		Sem.SetGraphName(t.val); 
	}

	void Statment() {
		Statment st = new Statment(); Token ifTn = null, elseTn = null; 
		Expect(1);
		st.Tok = Sem.OptTok(t); 
		if (la.kind == 6 || la.kind == 7) {
			if (la.kind == 6) {
				IfStatment(out ifTn);
			} else {
				ElseStatment(out elseTn);
			}
		}
		Expect(3);
		while (la.kind == 3) {
			Get();
		}
		while (la.kind == 2) {
			Get();
			st.Prop.Add(Sem.OptTok(t)); 
			Expect(3);
		}
		st.IfTok = Sem.OptTok(ifTn); st.ElseTok = Sem.OptTok(elseTn); Sem.AddStatment(st); 
	}

	void IfStatment(out Token tn) {
		Expect(6);
		Expect(1);
		tn = t; 
	}

	void ElseStatment(out Token tn) {
		Expect(7);
		tn = t; 
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		Sample();
		Expect(0);

	}
	
	static readonly bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x}

	};
} // end Parser


public class Errors {
	public int count = 0;                                    // number of errors detected
	public System.IO.TextWriter errorStream = Console.Out;   // error messages go to this stream
	public string errMsgFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text

	public virtual void SynErr (int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "ident expected"; break;
			case 2: s = "cond expected"; break;
			case 3: s = "eol expected"; break;
			case 4: s = "\"Begin\" expected"; break;
			case 5: s = "\"End\" expected"; break;
			case 6: s = "\"if\" expected"; break;
			case 7: s = "\"else\" expected"; break;
			case 8: s = "??? expected"; break;

			default: s = "error " + n; break;
		}
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}

	public virtual void SemErr (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}
	
	public virtual void SemErr (string s) {
		errorStream.WriteLine(s);
		count++;
	}
	
	public virtual void Warning (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
	}
	
	public virtual void Warning(string s) {
		errorStream.WriteLine(s);
	}
} // Errors


public class FatalError: Exception {
	public FatalError(string m): base(m) {}
}
