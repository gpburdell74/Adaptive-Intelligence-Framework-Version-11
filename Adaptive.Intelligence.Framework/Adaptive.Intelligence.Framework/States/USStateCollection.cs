namespace Adaptive.Intelligence.States
{
    /// <summary>
    /// Contains a list of the US State definitions.
    /// </summary>
    public sealed class USStateCollection : List<USState>, ICloneable
    {
        /// <summary>
        /// Gets the State by the postal abbreviation value.
        /// </summary>
        /// <param name="abbreviation">
        /// A string containing a standard 2-character postal abbreviation
        /// for the State.
        /// </param>
        /// <returns>
        /// A <see cref="USState"/> instance, if found; otherwise, returns
        /// <b>null</b>.
        /// </returns>
        public USState? GetStateByAbbreviation(string? abbreviation)
        {
            if (abbreviation == null)
            {
                return null;
            }
            else
            {
                IEnumerable<USState> query =
                    from states in this
                    where string.Equals(states.Abbreviation, abbreviation, StringComparison.OrdinalIgnoreCase)
                    select states;

                List<USState> list = [.. query];
                return list.Count == 0 ? null : list[0];
            }
        }
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        object ICloneable.Clone()
        {
            return Clone();
        }
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new <see cref="USStateCollection"/> that is a copy of this instance.
        /// </returns>
        public USStateCollection Clone()
        {
            USStateCollection collection = [.. this];
            return collection;
        }
    }
}