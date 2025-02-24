using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Graph.Models;

namespace MVCTutorial.Models
{
        [Table("ImageStore")]
        public class ImageStore
        {
            //public ImageStore()
            //{
            //    Products = new HashSet<Product>();
            //}

            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int ImageID { get; set; }
            public string ImageName { get; set; }
            public byte[] ImageByte { get; set; }
            public string ImagePath { get; set; }
            public bool? IsDeleted { get; set; }

            //public virtual ICollection<Product> Products { get; set; }
        }
}


