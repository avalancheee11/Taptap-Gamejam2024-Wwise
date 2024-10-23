using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class SimpleCaculateObject
{
	public static char[] SupportChars = new char[]
	{
		'+', '-', '*', '/',
		'²', '√', '(', ')',
	};

	/// <summary>
	/// 计算字符串表达式的值。
	/// </summary>
	/// <param name="exp">输入的表达式字符串</param>
	/// <returns>返回计算值，为double类型</returns>
	public static double Compute(string exp)
	{
		if (!checkRules(exp)) {
			throw new FormatException("字符串为空或不合法");
		}

		exp = exp.Replace(" ", string.Empty);
		//先把字符串转换成后缀表达式字符串数组
		string[] rpn = toRPN(toStrings(exp));

		//再计算后缀表达式
		Stack<double> stack = new Stack<double>(); //存放参与计算的数值、中间值、结果	

		//算法：利用foreach来扫描后缀表达式字符串数组，得到数值则直接压入栈中，
		//得到运算符则从栈顶取出两个数值进行运算，并把结果压入栈中。最终栈中留下一个数值，为计算结果。
		foreach (string oprator in rpn) {
			//为什么总是弹出两个数值？因为都是双目运算。
			//先弹出的是运算符右边的数，弹出两个数值后注意运算顺序。				
			switch (oprator) {
				case "+":
					//如果读取到运算符，则从stack中取出两个数值进行运算，并把运算结果压入stack。下同。
					stack.Push(stack.Pop() + stack.Pop());
					break;
				case "-":
					stack.Push(-stack.Pop() + stack.Pop());
					break;
				case "*":
					stack.Push(stack.Pop() * stack.Pop());
					break;
				case "/":
				{
					double right = stack.Pop();
					try {
						stack.Push(stack.Pop() / right);
					}
					catch (Exception e) {
						throw e; //除数为0时产生异常。
					}

					break;
				}
				case "^":
				{
					double right = stack.Pop();
					stack.Push(Math.Pow(stack.Pop(), right));
					break;
				}

				default: //后缀表达式数组中只有运算符和数值，没有圆括号。除了运算符，剩下的就是数值了
					stack.Push(double.Parse(oprator)); //如果读取到数值，则压入stack中
					break;
			}
		}

		//弹出最后的计算值并返回
		return stack.Pop();
	}

	/// <summary>
	/// 检查字符串，判断是否满足表达式的语法要求。
	/// </summary>
	/// <param name="exp">表达式字符串</param>
	/// <returns>字符串为空或不满足表达式语法要求时返回false，否则返回true</returns>
	public static bool checkRules(string exp)
	{
		if (string.IsNullOrWhiteSpace(exp)) {
			return false;
		}

		//去掉空格
		string noBlank = Regex.Replace(exp, " ", "");

		//Console.WriteLine(noBlank);

		//表达式字符串规则。规则之间有配合关系，以避免重叠。
		string no0 = @"[^\d\*\^\(\)+-/.]"; //规则0：不能出现运算符+-*/^、圆括号()、数字、小数点.之外的字符
		string no1 = @"^[^\d\(-]"; //规则1：开头不能是数字、左圆括号(、负号- 以外的字符
		string no2 = @"[^\d\)]$"; //规则2：结束不能是数字、右圆括号) 以外的字符
		string no3 = @"[\*\^+-/]{2}"; //规则3：+-*/^不能连续出现
		string no4 = @"[\D][.]|[.]\D"; //规则4：小数点前面或后面不能出现非数字字符
		string no5 = @"\)[\d\(]|[^\d\)]\)"; //规则5：右圆括号)后面不能出现数字、左圆括号(,前面不能出现除数字或右圆括号)以外的字符
		string no6 = @"\([^\d\(-]|[\d]\("; //规则6：左圆括号(后面不能出现除数字、左圆括号(、负号以外的字符,前面不能出现数字

		string pattern = no0 + "|" + no1 + "|" + no2 + "|" + no3 + "|" + no4 + "|" + no5 + "|" + no6;
		if (Regex.IsMatch(noBlank, pattern)) {
			return false;
		}

		//规则7：左圆括号(和右圆括号)必须成对出现
		int count = 0;
		foreach (char c in noBlank) {
			if (c == ')') {
				count++;
				continue;
			}

			if (c == '(') {
				count--;
				continue;
			}
		}

		if (count != 0) {
			Console.WriteLine("左右括号不匹配");
			return false;
		}

		return true;
	}


	/// <summary>
	/// 将表达式字符串转换成字符串数组，如"-2*57+6"转换成["-2","*","57","+","6"]。请自行确保表达式字符串正确。
	/// </summary>
	/// <param name="exp">表达式字符串</param>
	/// <returns>字符串数组</returns>
	public static string[] toStrings(string exp)
	{
		//通过添加分割符','把数字和其它字符分隔开。

		StringBuilder sb = new StringBuilder(exp);

		//去掉空格
		sb.Replace(" ", "");



		//负号与减号相同，不能与数字分开，需要特许处理：
		//1、字符串第一个"-"是负号
		//2、紧跟在"("后面的"-"是负号
		//3、如果负号后面直接跟着"("，把负号改为"-1*"。目的是把取负运算（单目运算）变成乘法运算（双目运算），免得以后要区分减法和取负。
		//4、把负号统一改为"?"			
		if (sb[0] == '-') {
			sb[0] = '?';
		}

		sb.Replace("(-", "(?");

		sb.Replace("?(", "?1*(");

		//添加分割符','把数字和其它字符分隔开。
		sb.Replace("+", ",+,");
		sb.Replace("-", ",-,");
		sb.Replace("*", ",*,");
		sb.Replace("/", ",/,");
		sb.Replace("(", "(,");
		sb.Replace(")", ",)");
		sb.Replace("^", ",^,");

		//分割之后，把'?'恢复成减号 '-'
		sb.Replace('?', '-');

		return sb.ToString().Split(',');
	}

	/// <summary>
	/// 生成后波兰表达式（后缀表达式）
	/// </summary>
	/// <param name="expStrings">字符串数组（中缀表达式）</param>
	/// <returns>字符串数组（后缀表达式）</returns>
	public static string[] toRPN(string[] expStrings)
	{
		Stack<string> stack = new Stack<string>();
		List<string> rpn = new List<string>();

		//基本思路：
		//遍历expStrings中的字符串：
		//1、如果不是字符（即数字）就直接放到列表rpn中；
		//2、如果是字符：
		//2.1、如果stack为空，把字符压入stack中；
		//2.2、如果stack不为空，把栈中优先级大于等于该字符的运算符全部弹出(直到碰到'('或stack为空)，放到rpn中；
		//2.2 如果字符是'('，直接压入
		//2.3 如果是')'，依次弹出stack中的字符串放入rpn中，直到碰到'(',弹出并抛弃'('；
		foreach (string item in expStrings) {
			//1、处理"("
			if (item == "(") {
				stack.Push(item);
				continue;
			}

			//2、处理运算符 +-*/^
			if ("+-*/^".Contains(item)) {
				if (stack.Count == 0) {
					stack.Push(item);
					continue;
				}

				if (getOrder(item[0]) > getOrder(stack.Peek()[0])) {
					stack.Push(item);
					continue;
				}
				else {
					while (stack.Count > 0 && getOrder(stack.Peek()[0]) >= getOrder(item[0]) && stack.Peek() != "(") {
						rpn.Add(stack.Pop());
					}

					stack.Push(item);
					continue;
				}
			}

			//3、处理")"
			if (item == ")") {
				while (stack.Peek() != "(") {
					rpn.Add(stack.Pop());
				}

				stack.Pop(); //抛弃"("
				continue;
			}

			//4、数据，直接放入rpn中
			rpn.Add(item);
		}

		//最后把stack中的运算符全部输出到rpn
		while (stack.Count > 0) {
			rpn.Add(stack.Pop());
		}


		//把字符串链表转换成字符串数组，并输出。
		return rpn.ToArray();

	}

	/// <summary>
	/// 获取运算符的优先级
	/// </summary>
	/// <param name="oprator">运算符</param>
	/// <returns>运算符的优先级。</returns>
	private static int getOrder(char oprator)
	{
		switch (oprator) {
			case '+':
			case '-':
				return 1;
			case '*':
			case '/':
				return 3;
			case '^':
				return 5;
			//case '(':
			//	return 10;					
			default:
				return -1;
		}
	}
}
