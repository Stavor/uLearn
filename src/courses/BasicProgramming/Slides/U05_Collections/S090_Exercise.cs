﻿namespace uLearn.Courses.BasicProgramming.Slides.U05_Collections
{
	[Slide("Практика", "{BBB4837D-48D5-4329-9C0C-D91E8288905F}")]
	public class S090_Exercise
	{
		/* 
		N-граммная модель текста
		==============

		В этот раз мы проведем обработку текста, который находится в файле [text.txt (zip)](text.zip). 
		Наша конечная цель — алгоритм продолжения текста, 
		который по нескольким введенным словам угадывает следующее на основе знаний, полученных из большого текстового массива.

		Очистка файла (1 балл)
		----------

		Разберите файл на слова и выведите 50 самых часто встречающихся слов.

		Считайте, что слова могут состоять из букв (char.IsLetter) и апострофа. Различия между прописными и строчными буквами игнорируйте.

		Считайте, что предложения разделены одним из следующих символов: `.!?;:()-`

		Удалите из файла артикли и наиболее часто встречающиеся предлоги (на основании результатов выполнения предыдущего задания).

		Биграммы (1 балл)
		--------

		Биграмма — это пара соседних слов предложения. 
		Например, из текста: "She stood up. Then she left." можно выделить следующие биграммы "She stood", "stood up", "Then she" и "she left", но не "up then".

		Составьте частотный словарь биграмм в тексте. Выведите двадцать наиболее часто встречающихся биграмм.

		Продолжение текста (1 балл)
		--------------

		Составьте алгоритм, который по данному слову W ищет наиболее часто встречающуюся биграмму (W, V) и выдает слово V.

		Продемонстрируйте работу этого алгоритма, построив по слову V1 предложение следующим образом: 
		найдите грамму (V1,V2) и выведите V2, затем граммы (V2, V3), (V3, V4) и так далее (выводить 10 слов).

		N-граммы (1 балл)
		--------

		N-грамма - это последовательность N слов в предложении. Модифицируйте алгоритм чтобы он мог учитывать не только биграммы, но 3-граммы. 
		Выведите по 10 самых частых 3-грамм.

		Модифицируйте алгоритм продолжения текста следующим образом: 
		
		1. По слову V1 найдите наиболее частую биграмму (V1, V2) и продолжите текст словом V2.
		2. Затем по паре слов V1 и V2 найти наиболее частую триграмму (V1, V2, V3) и продолжите текст словом V3.
		3. Затем по паре слов V2 и V3 найти наиболее частую триграмму (V2, V3, V4) и продолжите текст словом V4. И так далее.
		
		Если хотите, можете придумать как превратить алгоритм в вероятностный — так чтобы при каждом запуске генерировалась новая фраза,
		однако при этом все еще учитывалась статистика проанализированного текста.
 
		Применение N-граммных моделей
		----

		N-граммные модели текстов имеют широкое применение в различных задачах анализа текстов.
		Частотный словарь N-грамм можно учитывать при выборе наиболее удачного перевода фразы на другой язык.
		Поисковые системы используют N-граммные модели, чтобы предсказывать по введенному в строку поиска слову продолжение поискового запроса.
		Сравнивая частотные словари N-грамм двух текстов можно судить о степени схожести текстов.
		*/
	}
}