using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class YamSequencer {

	public static List<YamType> GenerateSequence(List<string> configEntries)
	{
		List<YamType> result = new List<YamType>();
		char[] splitter = ",".ToCharArray();

		foreach (string entry in configEntries) 
		{
			string[] parts = entry.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
			if (parts == null || parts.Length != 3) {
				LogErrorInConsole(ErrorMessageWrongFormat);
				continue;
			}

			int sequenceEndingYamType = 0;
			int precedingGoodYams = 0;
			int loops = 0;
			if (!int.TryParse(parts [0], out precedingGoodYams)
				|| !int.TryParse(parts[1], out sequenceEndingYamType) || !Enum.IsDefined(typeof(YamType), sequenceEndingYamType)
				|| !int.TryParse(parts [2], out loops)) 
			{
				LogErrorInConsole(ErrorMessageWrongFormat);
				continue;
			}

			AppendToSequence(ref result, precedingGoodYams, (YamType)sequenceEndingYamType, loops);
		}

		return result.Count > 0 ? result : GetFallbackSequence();
	}

	private const string ErrorMessageWrongFormat = "[ERROR] format error(s) in yam sequences config.";
	private const string ErrorMessageConfigMissing = "[ERROR] yam sequences config is missing.";
	private static void LogErrorInConsole(string message) 
	{
		Debug.LogException(new Exception(message));
	}

	private static void AppendToSequence(
		ref List<YamType> sequence, int precedingGoodYams, YamType sequenceEndingYamType, int loops)
	{
		for (int i = 0; i < loops; i++) 
		{
			sequence.AddRange(Enumerable.Repeat(YamType.GoodYam, precedingGoodYams).ToList()); 
			sequence.Add(sequenceEndingYamType);
		}
	}

	private static List<YamType> GetFallbackSequence()
	{
		LogErrorInConsole(ErrorMessageConfigMissing);

		List<YamType> result = new List<YamType>();
		AppendToSequence(ref result, 2, YamType.GoodYam, 10);
		return result;
	}
}
