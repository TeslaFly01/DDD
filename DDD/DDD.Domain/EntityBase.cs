using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain
{
    /// <summary>
    /// 业务实体都要继承此类
    /// </summary>
    public abstract class EntityBase : IEntity
    {
        private object key;



        #region Construcotrs


        protected EntityBase()
            : this(null)
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="key">An <see cref="System.Object"/> that 
        /// represents the primary identifier value for the 
        /// class.</param>
        protected EntityBase(object key)
        {
            this.key = key;
            if (this.key == null)
            {
                this.key = EntityBase.NewKey();
            }
            this.IsDel = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// An <see cref="System.Object"/> that represents the 
        /// primary identifier value for the class.
        /// </summary>
        public virtual object Key
        {
            get
            {
                return this.key;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Key", "The Key property cannot be set to null.");
                }
                this.key = value;
            }
        }

        #endregion

        #region NewKey

        public static object NewKey()
        {
            return Guid.NewGuid();
        }

        #endregion

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public virtual bool IsDel { get; set; }

        #region Equality Tests

        /// <summary>
        /// Determines whether the specified entity is equal to the 
        /// current instance.
        /// </summary>
        /// <param name="entity">An <see cref="System.Object"/> that 
        /// will be compared to the current instance.</param>
        /// <returns>True if the passed in entity is equal to the 
        /// current instance.</returns>
        public override bool Equals(object entity)
        {
            return entity != null
                && entity is EntityBase
                && this == (EntityBase)entity;
        }

        /// <summary>
        /// Operator overload for determining equality.
        /// </summary>
        /// <param name="base1">The first instance of an 
        /// <see cref="EntityBase"/>.</param>
        /// <param name="base2">The second instance of an 
        /// <see cref="EntityBase"/>.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(EntityBase base1,
            EntityBase base2)
        {
            // check for both null (cast to object or recursive loop)
            if ((object)base1 == null && (object)base2 == null)
            {
                return true;
            }

            // check for either of them == to null
            if ((object)base1 == null || (object)base2 == null)
            {
                return false;
            }

            if (!base1.Key.Equals(base2.Key))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Operator overload for determining inequality.
        /// </summary>
        /// <param name="base1">The first instance of an 
        /// <see cref="EntityBase"/>.</param>
        /// <param name="base2">The second instance of an 
        /// <see cref="EntityBase"/>.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(EntityBase base1,
            EntityBase base2)
        {
            return (!(base1 == base2));
        }

        /// <summary>
        /// Serves as a hash function for this type.
        /// </summary>
        /// <returns>A hash code for the current Key 
        /// property.</returns>
        public override int GetHashCode()
        {
            if (this.key != null)
            {
                return this.key.GetHashCode();
            }
            else
            {
                return 0;
            }
        }

        #endregion

    }
}
