using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarProjectServer.DAL.Entities
{
    /// <summary>
    /// Технические работы.
    /// </summary>
    public class TechnicalWork
    {
        /// <summary>
        /// Идентификатор технических работ.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Начало технических работ.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Конец технических работ.
        /// </summary>
        public DateTime End { get; set; }
    }
}
