﻿using uLearn;

namespace OOP.Slides.U01_DataTypes_And_Algorithms
{
	[Slide("RobotCommand", "{2FAF3A04-84BE-4A4A-9364-38A1AD023D73}")]
	public class S065_RobotCommand
	{
		/*
		Классы подобные RobotCommand иногда называют DTO — Data Transfer Object.

		Они обычно не содержат методов, а состоят только из полей (и, возможно, конструктора).
		Единственная цель таких объектов — передать данные из одной подсистемы в другую.

		Методы в C# и многих других языках способны возвращать лишь одно значение, поэтому в тех случаях, когда нужно вернуть несколько, приходится объединять их в DTO.
		Так, например, гипотетическая функция планирования движения робота могла бы возвращать экземпляр RobotCommand:
			
			RobotCommand CalculateNextCommand(); //функция планирования

		Еще одна выгода от существования RobotCommand станет ясна, когда у робота появится манипулятор, 
		пушка или любой другой актуатор, которым тоже нужно управлять. 
		В этом случае не придется менять ни сигнатуру метода Move, ни сигнатуру функции планирования движения робота.
		Всего лишь придется добавить еще несколько полей в RobotCommand.
		*/
		public class RobotCommand
		{
			public double Duration;
			public double Velocity;
			public double AngularVelocity;

			// новые поля для управления роботом:
			public double GunAngularVelocity;
			public bool FireGun;
		}
	}
}