﻿namespace uLearn.Courses.Linq.Slides.LINQ
{
	[Slide("Задача для самостоятельного решения", "{FCE846DF-84C4-426B-9D5C-CC99E2FBA454}")]
	public class S200_HomeExercise : SlideTestBase
	{
		/*
		Напишите программу, которая измеряет сходство двух текстов, подсчитывая коэффициент Жаккара по множествам N-грамм.
		
		### N-граммы
		
		[N-граммой](https://ru.wikipedia.org/wiki/N-%D0%B3%D1%80%D0%B0%D0%BC%D0%BC) назовем последовательность из N слов, приведенных к нижнему регистру.
		Будем говорить, что N-грамма встретилась в тексте, если все ее слова встречаются последовательно друг за другом в одном предложении.

		В данной задаче считайте, что предложения всегда отделяются друг от друга точкой одним или несколькими следующими символами: `.!?()[]{}`.
		То есть фразу "А. С. Пушкин был... [самым] настоящим (!) поэтом" в рамках этой задачи стоит считать шестью предложениями: "А", "C", "Пушкин был", "самым", "настоящим", "поэтом".

		При разбиении текста на слова, считайте, что слова могут состоять лишь из символов, входящих в категорию [L (Letter)](http://en.wikipedia.org/wiki/Unicode_character_property#General_Category) 
		в таблице Unicode. Конструкции \p и \P в регулярных выражениях могут оказаться полезными в этой задаче.
		
		### Коэффициент Жаккара

		Меру сходства двух текстов можно определить с помощью так называемого
		[коэффициента Жаккара](https://ru.wikipedia.org/wiki/%D0%9A%D0%BE%D1%8D%D1%84%D1%84%D0%B8%D1%86%D0%B8%D0%B5%D0%BD%D1%82_%D0%96%D0%B0%D0%BA%D0%BA%D0%B0%D1%80%D0%B0) 
		следующим образом:

		<span class="tex">freq(g, T)</span> — это количество вхождений граммы g в текст T.
		
		<span class="tex">N_T</span> — множество всех различных N-грамм текста T.
		
		<span class="tex">
			common(T_1, T_2) = \displaystyle \sum_{g \in N_{T_1 + T_2}} \min(freq(g, T_1), freq(g, T_2))
		</span>
		
		<span class="tex">
			J(T_1, T_2) = \displaystyle \frac{common(T_1, T_2)}{|N_{T_1}| + |N_{T_2}| - common(T_1, T_2)}
		</span>

		### Требование к оформлению

		Выполните решение в виде консольного приложения, принимающего три аргумента командной строки:
		
		* путь до файла с текстом <span class="tex">T_1</span>
		* путь до файла со текстом <span class="tex">T_2</span>
		* число N — размер N-грамм.

		Файлы с текстами — в кодировке UTF-8.

		Программа должна выводить на консоль одно число — значение <span class="tex">J(T_1, T_2)</span>.
		*/

	}
}