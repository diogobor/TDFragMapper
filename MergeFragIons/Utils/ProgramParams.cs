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

namespace TDFragMapper.Utils
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
        /// Contains information about PTM
        /// </summary>
        [ProtoMember(2)]
        public string SequenceInformation { get; set; }
        /// <summary>
        /// List<(MS/MS Data, Fragmentation Method, Activation Level, Precursor Charge State, Replicate, Intensity Data)>
        /// </summary>
        [ProtoMember(3)]
        public List<(string, string, string, int, int, string)> InputFileList { get; set; }
        [ProtoMember(4)]
        public byte[] ProcessingTime { get; set; }
        [ProtoMember(5)]
        public byte[] ProgramVersion { get; set; }
        [ProtoMember(6)]
        public bool HasIntensities { get; set; }
        [ProtoMember(7)]
        public string ModificationSites { get; set; }

        public string msgErrorInputFiles { get; set; }

        /// <summary>
        /// Null Constructor for serializing Class
        /// </summary>
        public ProgramParams() { }
    }
}
