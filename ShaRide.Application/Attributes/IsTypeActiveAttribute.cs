using System;

namespace ShaRide.Application.Attributes
{
    /// <summary>
    /// Attribute that defines is applied target is active or not.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum |
                    AttributeTargets.Interface | AttributeTargets.Constructor | AttributeTargets.Method |
                    AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event |
                    AttributeTargets.Delegate
        , Inherited = false)]
    public class IsTypeActiveAttribute : Attribute
    {
        private readonly bool _isActive;

        public IsTypeActiveAttribute(bool isActive = false)
        {
            this._isActive = isActive;
        }

        public bool IsActive => _isActive;
    }
}