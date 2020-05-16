/**
 * Program:     ProteoCombiner - Integrating bottom-up & top-down proteomics data
 * Author:      Diogo Borges Lima
 * Created:     4/3/2019
 * Update by:   Diogo Borges Lima
 * Description: Class responsable for saving all program parameters
 */
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeFragIons.Utils
{
    [Serializable]
    [ProtoContract]
    public class ProgramParams
    {
        /// <summary>
        /// Public variables
        /// </summary>
        [ProtoMember(1)]
        public string ProteinSequenceFile { get; set; }
        /// <summary>
        /// List<(MS/MS Data, Fragmentation Method, Activation Level, Precursor Charge State, Replicate)>
        /// </summary>
        [ProtoMember(2)]
        public List<(string, string, string, int, int)> InputFileList { get; set; }
        [ProtoMember(3)]
        public byte[] ProcessingTime { get; set; }
        [ProtoMember(4)]
        public byte[] ProgramVersion { get; set; }

        public string msgErrorInputFiles { get; set; }

        /// <summary>
        /// Null Constructor for serializing Class
        /// </summary>
        public ProgramParams() { }
    }
}
