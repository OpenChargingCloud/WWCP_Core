/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A energy meter admin status update.
    /// </summary>
    public readonly struct EnergyMeterAdminStatusUpdate : IEquatable<EnergyMeterAdminStatusUpdate>,
                                                          IComparable<EnergyMeterAdminStatusUpdate>,
                                                          IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the energy meter.
        /// </summary>
        public EnergyMeter_Id                             Id            { get; }

        /// <summary>
        /// The new timestamped admin status of the energy meter.
        /// </summary>
        public Timestamped<EnergyMeterAdminStatusTypes>   NewStatus     { get; }

        /// <summary>
        /// The optional old timestamped admin status of the energy meter.
        /// </summary>
        public Timestamped<EnergyMeterAdminStatusTypes>?  OldStatus     { get; }

        /// <summary>
        /// An optional data source or context for this energy meter status update.
        /// </summary>
        public Context?                                   DataSource    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new energy meter admin status update.
        /// </summary>
        /// <param name="Id">The unique identification of the energy meter.</param>
        /// <param name="NewStatus">The new timestamped admin status of the energy meter.</param>
        /// <param name="OldStatus">The optional old timestamped admin status of the energy meter.</param>
        /// <param name="DataSource">An optional data source or context for the energy meter status update.</param>
        public EnergyMeterAdminStatusUpdate(EnergyMeter_Id                             Id,
                                            Timestamped<EnergyMeterAdminStatusTypes>   NewStatus,
                                            Timestamped<EnergyMeterAdminStatusTypes>?  OldStatus    = null,
                                            Context?                                   DataSource   = null)

        {

            this.Id          = Id;
            this.NewStatus   = NewStatus;
            this.OldStatus   = OldStatus;
            this.DataSource  = DataSource;

            unchecked
            {

                hashCode = Id.         GetHashCode()       * 7 ^
                           NewStatus.  GetHashCode()       * 5 ^
                          (OldStatus?. GetHashCode() ?? 0) * 3 ^
                          (DataSource?.GetHashCode() ?? 0);

            }

        }

        #endregion


        #region (static) Snapshot(EnergyMeter, DataSource = null)

        /// <summary>
        /// Take a snapshot of the current energy meter admin status.
        /// </summary>
        /// <param name="EnergyMeter">A energy meter.</param>
        /// <param name="DataSource">An optional data source or context for the EVSE status update.</param>
        public static EnergyMeterAdminStatusUpdate Snapshot(IEnergyMeter  EnergyMeter,
                                                            Context?      DataSource   = null)

            => new (EnergyMeter.Id,
                    EnergyMeter.AdminStatus,
                    EnergyMeter.AdminStatusSchedule().Skip(1).FirstOrDefault(),
                    DataSource);

        #endregion


        #region Operator overloading

        #region Operator == (EnergyMeterAdminStatusUpdate1, EnergyMeterAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterAdminStatusUpdate1">A energy meter admin status update.</param>
        /// <param name="EnergyMeterAdminStatusUpdate2">Another energy meter admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EnergyMeterAdminStatusUpdate EnergyMeterAdminStatusUpdate1,
                                           EnergyMeterAdminStatusUpdate EnergyMeterAdminStatusUpdate2)

            => EnergyMeterAdminStatusUpdate1.Equals(EnergyMeterAdminStatusUpdate2);

        #endregion

        #region Operator != (EnergyMeterAdminStatusUpdate1, EnergyMeterAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterAdminStatusUpdate1">A energy meter admin status update.</param>
        /// <param name="EnergyMeterAdminStatusUpdate2">Another energy meter admin status update.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EnergyMeterAdminStatusUpdate EnergyMeterAdminStatusUpdate1,
                                           EnergyMeterAdminStatusUpdate EnergyMeterAdminStatusUpdate2)

            => !EnergyMeterAdminStatusUpdate1.Equals(EnergyMeterAdminStatusUpdate2);

        #endregion

        #region Operator <  (EnergyMeterAdminStatusUpdate1, EnergyMeterAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterAdminStatusUpdate1">A energy meter admin status update.</param>
        /// <param name="EnergyMeterAdminStatusUpdate2">Another energy meter admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (EnergyMeterAdminStatusUpdate EnergyMeterAdminStatusUpdate1,
                                          EnergyMeterAdminStatusUpdate EnergyMeterAdminStatusUpdate2)

            => EnergyMeterAdminStatusUpdate1.CompareTo(EnergyMeterAdminStatusUpdate2) < 0;

        #endregion

        #region Operator <= (EnergyMeterAdminStatusUpdate1, EnergyMeterAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterAdminStatusUpdate1">A energy meter admin status update.</param>
        /// <param name="EnergyMeterAdminStatusUpdate2">Another energy meter admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (EnergyMeterAdminStatusUpdate EnergyMeterAdminStatusUpdate1,
                                           EnergyMeterAdminStatusUpdate EnergyMeterAdminStatusUpdate2)

            => EnergyMeterAdminStatusUpdate1.CompareTo(EnergyMeterAdminStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (EnergyMeterAdminStatusUpdate1, EnergyMeterAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterAdminStatusUpdate1">A energy meter admin status update.</param>
        /// <param name="EnergyMeterAdminStatusUpdate2">Another energy meter admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (EnergyMeterAdminStatusUpdate EnergyMeterAdminStatusUpdate1,
                                          EnergyMeterAdminStatusUpdate EnergyMeterAdminStatusUpdate2)

            => EnergyMeterAdminStatusUpdate1.CompareTo(EnergyMeterAdminStatusUpdate2) > 0;

        #endregion

        #region Operator >= (EnergyMeterAdminStatusUpdate1, EnergyMeterAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterAdminStatusUpdate1">A energy meter admin status update.</param>
        /// <param name="EnergyMeterAdminStatusUpdate2">Another energy meter admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (EnergyMeterAdminStatusUpdate EnergyMeterAdminStatusUpdate1,
                                           EnergyMeterAdminStatusUpdate EnergyMeterAdminStatusUpdate2)

            => EnergyMeterAdminStatusUpdate1.CompareTo(EnergyMeterAdminStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<EnergyMeterAdminStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two energy meter admin status updates.
        /// </summary>
        /// <param name="Object">A energy meter admin status update to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EnergyMeterAdminStatusUpdate energyMeterAdminStatusUpdate
                   ? CompareTo(energyMeterAdminStatusUpdate)
                   : throw new ArgumentException("The given object is not a energy meter admin status update!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EnergyMeterAdminStatusUpdate)

        /// <summary>
        /// Compares two energy meter admin status updates.
        /// </summary>
        /// <param name="EnergyMeterAdminStatusUpdate">A energy meter admin status update to compare with.</param>
        public Int32 CompareTo(EnergyMeterAdminStatusUpdate EnergyMeterAdminStatusUpdate)
        {

            var c = Id.             CompareTo(EnergyMeterAdminStatusUpdate.Id);

            if (c == 0)
                c = NewStatus.      CompareTo(EnergyMeterAdminStatusUpdate.NewStatus);

            if (c == 0 && OldStatus.HasValue && EnergyMeterAdminStatusUpdate.OldStatus.HasValue)
                c = OldStatus.Value.CompareTo(EnergyMeterAdminStatusUpdate.OldStatus.Value);

            if (c == 0 && DataSource is not null && EnergyMeterAdminStatusUpdate.DataSource is not null)
                c = DataSource.     CompareTo(EnergyMeterAdminStatusUpdate.DataSource);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EnergyMeterAdminStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two energy meter admin status updates for equality.
        /// </summary>
        /// <param name="Object">A energy meter admin status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EnergyMeterAdminStatusUpdate energyMeterAdminStatusUpdate &&
                   Equals(energyMeterAdminStatusUpdate);

        #endregion

        #region Equals(EnergyMeterAdminStatusUpdate)

        /// <summary>
        /// Compares two energy meter admin status updates for equality.
        /// </summary>
        /// <param name="EnergyMeterAdminStatusUpdate">A energy meter admin status update to compare with.</param>
        public Boolean Equals(EnergyMeterAdminStatusUpdate EnergyMeterAdminStatusUpdate)

            => Id.       Equals(EnergyMeterAdminStatusUpdate.Id)        &&
               NewStatus.Equals(EnergyMeterAdminStatusUpdate.NewStatus) &&

            ((!OldStatus.HasValue     && !EnergyMeterAdminStatusUpdate.OldStatus.HasValue) ||
              (OldStatus.HasValue     &&  EnergyMeterAdminStatusUpdate.OldStatus.HasValue     && OldStatus.Value.Equals(EnergyMeterAdminStatusUpdate.OldStatus.Value))) &&

            (( DataSource is null     &&  EnergyMeterAdminStatusUpdate.DataSource is null) ||
              (DataSource is not null &&  EnergyMeterAdminStatusUpdate.DataSource is not null && DataSource.     Equals(EnergyMeterAdminStatusUpdate.DataSource)));

        #endregion

        #endregion

        #region (override) GetHashCode()

        private readonly Int32 hashCode;

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        public override Int32 GetHashCode()
            => hashCode;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{Id}: {(OldStatus.HasValue ? $"'{OldStatus.Value}' -> " : "")}'{NewStatus}'{(DataSource is not null ? $" ({DataSource})" : "")}";

        #endregion

    }

}
