/**
 * Program:     ProteinAnnotation
 * Author:      Diogo Borges Lima
 * Created:     5/14/2019
 * Update by:   Diogo Borges Lima
 * Description: Util class
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProteinAnnotation.Utils
{
    public static class Util
    {
        public static string CleanPeptide(string peptide)
        {
            string cleanedPeptide = Regex.Replace(peptide, @"^[A-Z|\-|\*]+\.", "");
            cleanedPeptide = Regex.Replace(cleanedPeptide, @"\.[A-Z|\-|\*]+$", "");
            cleanedPeptide = cleanedPeptide.Replace("*", "");
            cleanedPeptide = cleanedPeptide.Replace("#", "");
            cleanedPeptide = Regex.Replace(cleanedPeptide, @"\([0-9|\.|\+|\-| |a-z|A-Z]*\)", "");
            cleanedPeptide = Regex.Replace(cleanedPeptide, @"\[[0-9|\.|\+|\-| |a-z|A-Z]*\]", "");

            return cleanedPeptide;
        }
    }
}
