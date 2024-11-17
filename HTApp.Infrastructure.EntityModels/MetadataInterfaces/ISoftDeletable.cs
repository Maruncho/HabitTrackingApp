using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTApp.Infrastructure.EntityModels;

//Helps with the repositories
public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }
}

//ef core requires classes in templates
public abstract class SoftDeletable : ISoftDeletable
{
    public abstract bool IsDeleted { get; set; }
}
