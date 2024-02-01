/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// An energy meter status update.
    /// </summary>
    public readonly struct EnergyMeterStatusUpdate : IEquatable<EnergyMeterStatusUpdate>,
                                                     IComparable<EnergyMeterStatusUpdate>,
                                                     IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the energy meter.
        /// </summary>
        public EnergyMeter_Id                        Id            { get; }

        /// <summary>
        /// The new timestamped status of the energy meter.
        /// </summary>
        public Timestamped<EnergyMeterStatusTypes>   NewStatus     { get; }

        /// <summary>
        /// The old timestamped status of the energy meter.
        /// </summary>
        public Timestamped<EnergyMeterStatusTypes>?  OldStatus     { get; }

        /// <summary>
        /// An optional data source or context for this energy meter status update.
        /// </summary>
        public Context?                              DataSource    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new energy meter status update.
        /// </summary>
        /// <param name="Id">The unique identification of the energy meter.</param>
        /// <param name="NewStatus">The new timestamped status of the energy meter.</param>
        /// <param name="OldStatus">The optional old timestamped status of the energy meter.</param>
        /// <param name="DataSource">An optional data source or context for the energy meter status update.</param>
        public EnergyMeterStatusUpdate(EnergyMeter_Id                        Id,
                                           Timestamped<EnergyMeterStatusTypes>   NewStatus,
                                           Timestamped<EnergyMeterStatusTypes>?  OldStatus    = null,
                                           Context?                                  DataSource   = null)

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
        /// Take a snapshot of the current energy meter status.
        /// </summary>
        /// <param name="EnergyMeter">An energy meter.</param>
        /// <param name="DataSource">An optional data source or context for the energy meter status update.</param>
        public static EnergyMeterStatusUpdate Snapshot(IEnergyMeter  EnergyMeter,
                                                           Context?          DataSource   = null)

            => new (EnergyMeter.Id,
                    EnergyMeter.Status,
                    EnergyMeter.StatusSchedule().Skip(1).FirstOrDefault(),
                    DataSource);

        #endregion


        #region Operator overloading

        #region Operator == (EnergyMeterStatusUpdate1, EnergyMeterStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterStatusUpdate1">An energy meter status update.</param>
        /// <param name="EnergyMeterStatusUpdate2">Another energy meter status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EnergyMeterStatusUpdate EnergyMeterStatusUpdate1,
                                           EnergyMeterStatusUpdate EnergyMeterStatusUpdate2)

            => EnergyMeterStatusUpdate1.Equals(EnergyMeterStatusUpdate2);

        #endregion

        #region Operator != (EnergyMeterStatusUpdate1, EnergyMeterStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterStatusUpdate1">An energy meter status update.</param>
        /// <param name="EnergyMeterStatusUpdate2">Another energy meter status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EnergyMeterStatusUpdate EnergyMeterStatusUpdate1,
                                           EnergyMeterStatusUpdate EnergyMeterStatusUpdate2)

            => !EnergyMeterStatusUpdate1.Equals(EnergyMeterStatusUpdate2);

        #endregion

        #region Operator <  (EnergyMeterStatusUpdate1, EnergyMeterStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterStatusUpdate1">An energy meter status update.</param>
        /// <param name="EnergyMeterStatusUpdate2">Another energy meter status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EnergyMeterStatusUpdate EnergyMeterStatusUpdate1,
                                          EnergyMeterStatusUpdate EnergyMeterStatusUpdate2)

            => EnergyMeterStatusUpdate1.CompareTo(EnergyMeterStatusUpdate2) < 0;

        #endregion

        #region Operator <= (EnergyMeterStatusUpdate1, EnergyMeterStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterStatusUpdate1">An energy meter status update.</param>
        /// <param name="EnergyMeterStatusUpdate2">Another energy meter status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EnergyMeterStatusUpdate EnergyMeterStatusUpdate1,
                                           EnergyMeterStatusUpdate EnergyMeterStatusUpdate2)

            => EnergyMeterStatusUpdate1.CompareTo(EnergyMeterStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (EnergyMeterStatusUpdate1, EnergyMeterStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterStatusUpdate1">An energy meter status update.</param>
        /// <param name="EnergyMeterStatusUpdate2">Another energy meter status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EnergyMeterStatusUpdate EnergyMeterStatusUpdate1,
                                          EnergyMeterStatusUpdate EnergyMeterStatusUpdate2)

            => EnergyMeterStatusUpdate1.CompareTo(EnergyMeterStatusUpdate2) > 0;

        #endregion

        #region Operator >= (EnergyMeterStatusUpdate1, EnergyMeterStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterStatusUpdate1">An energy meter status update.</param>
        /// <param name="EnergyMeterStatusUpdate2">Another energy meter status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EnergyMeterStatusUpdate EnergyMeterStatusUpdate1,
                                           EnergyMeterStatusUpdate EnergyMeterStatusUpdate2)

            => EnergyMeterStatusUpdate1.CompareTo(EnergyMeterStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<EnergyMeterStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two energy meter status updates.
        /// </summary>
        /// <param name="Object">An energy meter status update to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EnergyMeterStatusUpdate energyMeterStatusUpdate
                   ? CompareTo(energyMeterStatusUpdate)
                   : throw new ArgumentException("The given object is not a energy meter status update!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EnergyMeterStatusUpdate)

        /// <summary>
        /// Compares two energy meter status updates.
        /// </summary>
        /// <param name="EnergyMeterStatusUpdate">An energy meter status update to compare with.</param>
        public Int32 CompareTo(EnergyMeterStatusUpdate EnergyMeterStatusUpdate)
        {

            var c = Id.             CompareTo(EnergyMeterStatusUpdate.Id);

            if (c == 0)
                c = NewStatus.      CompareTo(EnergyMeterStatusUpdate.NewStatus);

            if (c == 0 && OldStatus.HasValue && EnergyMeterStatusUpdate.OldStatus.HasValue)
                c = OldStatus.Value.CompareTo(EnergyMeterStatusUpdate.OldStatus.Value);

            if (c == 0 && DataSource is not null && EnergyMeterStatusUpdate.DataSource is not null)
                c = DataSource.     CompareTo(EnergyMeterStatusUpdate.DataSource);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EnergyMeterStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two energy meter status updates for equality.
        /// </summary>
        /// <param name="Object">An energy meter status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EnergyMeterStatusUpdate energyMeterStatusUpdate &&
                   Equals(energyMeterStatusUpdate);

        #endregion

        #region Equals(EnergyMeterStatusUpdate)

        /// <summary>
        /// Compares two energy meter status updates for equality.
        /// </summary>
        /// <param name="EnergyMeterStatusUpdate">An energy meter status update to compare with.</param>
        public Boolean Equals(EnergyMeterStatusUpdate EnergyMeterStatusUpdate)

            => Id.       Equals(EnergyMeterStatusUpdate.Id)        &&
               NewStatus.Equals(EnergyMeterStatusUpdate.NewStatus) &&

            ((!OldStatus.HasValue     && !EnergyMeterStatusUpdate.OldStatus.HasValue) ||
              (OldStatus.HasValue     &&  EnergyMeterStatusUpdate.OldStatus.HasValue     && OldStatus.Value.Equals(EnergyMeterStatusUpdate.OldStatus.Value))) &&

            (( DataSource is null     &&  EnergyMeterStatusUpdate.DataSource is null) ||
              (DataSource is not null &&  EnergyMeterStatusUpdate.DataSource is not null && DataSource.     Equals(EnergyMeterStatusUpdate.DataSource)));

        #endregion

        #endregion

        #region (override) GetHashCode()

        private readonly Int32 hashCode;

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
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
