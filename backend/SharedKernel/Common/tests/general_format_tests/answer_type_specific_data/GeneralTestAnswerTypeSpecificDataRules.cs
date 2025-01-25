using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Common.tests.general_format_tests.answer_type_specific_data;

internal static class GeneralTestAnswerTypeSpecificDataRules
{
    public const int
        AnswerMinLength = 5,
        AnswerMaxLength = 500;
    public const int
        NonTextDataMaxLength = 500;
    public static bool IsStringCorrectAnswerText(string str, out int length) {
        length = string.IsNullOrWhiteSpace(str) ? 0 : str.Length;
        if (length < AnswerMinLength || length > AnswerMaxLength) {
            return false;
        }
        return true;
    }
    public static bool IsStringCorrectNonTextItem(string str, out int length) {
        length = string.IsNullOrWhiteSpace(str) ? 0 : str.Length;
        if (length <= 0 || length > NonTextDataMaxLength) {
            return false;
        }
        return true;
    }
}
