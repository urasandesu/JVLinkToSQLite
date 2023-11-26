// JVLinkToSQLite は、JRA-VAN データラボが提供する競馬データを SQLite データベースに変換するツールです。
// 
// Copyright (C) 2023 Akira Sugiura
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// 
// Additional permission under GNU GPL version 3 section 7
// 
// If you modify this Program, or any covered work, by linking or combining it with 
// ObscUra (or a modified version of that library), containing parts covered 
// by the terms of ObscUra's license, the licensors of this Program grant you 
// additional permission to convey the resulting work.

using JVDTLabLib;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Security.Cryptography;
using static Urasandesu.JVLinkToSQLite.JVOperationMessenger;

namespace Urasandesu.JVLinkToSQLite.JVLinkWrappers
{
    internal class JVLinkService : IJVLinkService
    {
        private readonly IJVServiceOperationListener _listener;
        private readonly IJVLink _jvLink;
        private readonly SIDBuilder _sidBldr;
        private readonly IEncryptor _encry;
        private JVWatchEventDispatcher _dispatcher;
        private static readonly Encoding ShiftJisEncoding = Encoding.GetEncoding(932);

        public JVLinkService(IJVServiceOperationListener listener, JVLink jvLink, SIDBuilder sidBldr, IEncryptor encry)
        {
            _listener = listener;
            _jvLink = jvLink;
            _sidBldr = sidBldr;
            _encry = encry;

            var initRslt = JVInit(_sidBldr.SID);
            if (initRslt.Interpretation.Failed)
            {
                throw new JVLinkException(initRslt);
            }

            jvLink.JVEvtPay += _jvLink_JVEvtPay;
            jvLink.JVEvtJockeyChange += _jvLink_JVEvtJockeyChange;
            jvLink.JVEvtWeather += _jvLink_JVEvtWeather;
            jvLink.JVEvtCourseChange += _jvLink_JVEvtCourseChange;
            jvLink.JVEvtAvoid += _jvLink_JVEvtAvoid;
            jvLink.JVEvtTimeChange += _jvLink_JVEvtTimeChange;
            jvLink.JVEvtWeight += _jvLink_JVEvtWeight;
        }

        private JVInitResult JVInit(string sid)
        {
            VerboseApiCalling(_listener, this, nameof(JVLink), nameof(JVLink.JVInit)/*, sid*/);
            Debug(_listener, this, args => $"Encrypted SID：{_encry.Encrypt((string)args[0])}", sid);
            var returnCode = _jvLink.JVInit(sid);
            VerboseApiCalled(_listener, this, nameof(JVLink), nameof(JVLink.JVInit), returnCode);

            var rslt = new JVInitResult();
            rslt.SetArguments(sid);
            rslt.SetReturnCode(returnCode);
            return rslt;
        }

        public JVOpenResult JVOpen(JVDataSpec dataSpec, JVDataSpecKey dataSpecKey, JVOpenOptions openOption)
        {
            if (dataSpec == null)
            {
                throw new ArgumentNullException(nameof(dataSpec));
            }
            if (dataSpecKey == null)
            {
                throw new ArgumentNullException(nameof(dataSpecKey));
            }
            var dataspec = dataSpec.Value;
            var fromdate = dataSpecKey.GetKey();
            var option = (int)openOption;
            var readCount = default(int);
            var downloadCount = default(int);
            var lastFileTimestamp = default(string);

            VerboseApiCalling(_listener, this, nameof(JVLink), nameof(JVLink.JVOpen), dataspec, fromdate, option);
            var returnCode = _jvLink.JVOpen(dataspec, fromdate, option, ref readCount, ref downloadCount, out lastFileTimestamp);
            VerboseApiCalled(_listener, this, nameof(JVLink), nameof(JVLink.JVOpen), returnCode, readCount, downloadCount, lastFileTimestamp);

            var rslt = new JVOpenResult();
            rslt.SetArguments(dataspec, fromdate, option);
            rslt.SetReturnCode(returnCode);
            rslt.SetJVLink(_jvLink);
            rslt.SetDataSpec(dataSpec);
            rslt.SetOpenOption(openOption);
            rslt.SetReadCount(readCount);
            rslt.SetDownloadCount(downloadCount);
            rslt.SetLastFileTimestamp(lastFileTimestamp);
            return rslt;
        }

        public JVOpenResult JVRTOpen(JVDataSpec dataSpec, JVDataSpecKey dataSpecKey)
        {
            if (dataSpec == null)
            {
                throw new ArgumentNullException(nameof(dataSpec));
            }
            if (dataSpecKey == null)
            {
                throw new ArgumentNullException(nameof(dataSpecKey));
            }
            var dataspec = dataSpec.Value;
            var key = dataSpecKey.GetKey();

            VerboseApiCalling(_listener, this, nameof(JVLink), nameof(JVLink.JVRTOpen), dataspec, key);
            var returnCode = _jvLink.JVRTOpen(dataspec, key);
            VerboseApiCalled(_listener, this, nameof(JVLink), nameof(JVLink.JVRTOpen), returnCode);

            var rslt = new JVOpenResult();
            rslt.SetArguments(dataspec, key);
            rslt.SetReturnCode(returnCode);
            rslt.SetJVLink(_jvLink);
            rslt.SetDataSpec(dataSpec);
            rslt.SetOpenOption(JVOpenOptions.RealTime);
            rslt.SetReadCount(1);
            rslt.SetDownloadCount(-1);
            rslt.SetLastFileTimestamp(null);
            return rslt;
        }

        public JVReadResult JVRead(JVOpenResult openRslt)
        {
            if (openRslt == null)
            {
                throw new ArgumentNullException(nameof(openRslt));
            }
            var size = openRslt.DataSpec.MaxLengthInByte;
            var buff = new string('\0', size);
            var fileName = default(string);

            VerboseApiCalling(_listener, this, nameof(JVLink), nameof(JVLink.JVRead));
            var returnCode = _jvLink.JVRead(out buff, out size, out fileName);
            VerboseApiCalled(_listener, this, nameof(JVLink), nameof(JVLink.JVRead), returnCode, buff, size, fileName);

            var rslt = new JVReadResult();
            rslt.SetArguments();
            rslt.SetReturnCode(returnCode);
            rslt.SetDataSpec(openRslt.DataSpec);
            rslt.SetOpenOption(openRslt.OpenOption);
            rslt.SetBuffer(buff);
            rslt.SetBufferSize(size);
            rslt.SetFileName(fileName);
            return rslt;
        }

        public JVReadResult JVGets(JVOpenResult openRslt)
        {
            if (openRslt == null)
            {
                throw new ArgumentNullException(nameof(openRslt));
            }

            var obuff = (object)IntPtr.Zero;
            var size = openRslt.DataSpec.MaxLengthInByte;
            var fileName = default(string);
            VerboseApiCalling(_listener, this, nameof(JVLink), nameof(JVLink.JVGets), size);
            var returnCode = _jvLink.JVGets(ref obuff, size, out fileName);
            VerboseApiCalled(_listener, this, nameof(JVLink), nameof(JVLink.JVGets), returnCode, RuntimeHelpers.GetHashCode(obuff), fileName);
            var buff = default(byte[]);
            if (0 < returnCode)
            {
                var psa = IntPtr.Size == 4 ? (IntPtr)(int)obuff : (IntPtr)(long)obuff;
                try
                {
                    buff = new byte[returnCode];
                    var pvData = UnsafeNativeMethods.SafeArrayAccessData(psa);
                    Marshal.Copy(pvData, buff, 0, buff.Length);
                    UnsafeNativeMethods.SafeArrayUnaccessData(psa);
                }
                finally
                {
                    UnsafeNativeMethods.SafeArrayDestroy(psa);
                }
            }
            else
            {
                obuff = null;
                buff = new byte[0];
            }

            var rslt = new JVReadResult();
            rslt.SetArguments();
            rslt.SetReturnCode(returnCode);
            rslt.SetDataSpec(openRslt.DataSpec);
            rslt.SetOpenOption(openRslt.OpenOption);
            rslt.SetBuffer(ShiftJisEncoding.GetString(buff));
            rslt.SetBufferSize(size);
            rslt.SetFileName(fileName);
            return rslt;
        }

        public JVSetUIPropertiesResult JVSetUIProperties()
        {
            VerboseApiCalling(_listener, this, nameof(JVLink), nameof(JVLink.JVSetUIProperties));
            var returnCode = _jvLink.JVSetUIProperties();
            VerboseApiCalled(_listener, this, nameof(JVLink), nameof(JVLink.JVSetUIProperties), returnCode);

            var rslt = new JVSetUIPropertiesResult();
            rslt.SetArguments();
            rslt.SetReturnCode(returnCode);
            return rslt;
        }

        public JVStatusResult JVStatus(JVOpenResult openRslt)
        {
            if (openRslt == null)
            {
                throw new ArgumentNullException(nameof(openRslt));
            }

            VerboseApiCalling(_listener, this, nameof(JVLink), nameof(JVLink.JVStatus));
            var returnCode = _jvLink.JVStatus();
            VerboseApiCalled(_listener, this, nameof(JVLink), nameof(JVLink.JVStatus), returnCode);

            var rslt = new JVStatusResult();
            rslt.SetArguments();
            rslt.SetReturnCode(returnCode);
            rslt.SetDownloadCount(openRslt.DownloadCount);
            return rslt;
        }

        public void JVCancel(JVOpenResult openRslt)
        {
            _jvLink.JVCancel();
        }

        public void JVSkip(JVOpenResult openRslt)
        {
            _jvLink.JVSkip();
        }

        public JVWatchEventResult JVWatchEvent(JVWatchEventDispatcher dispatcher)
        {
            VerboseApiCalling(_listener, this, nameof(JVLink), nameof(JVLink.JVWatchEvent));
            var returnCode = _jvLink.JVWatchEvent();
            VerboseApiCalled(_listener, this, nameof(JVLink), nameof(JVLink.JVWatchEvent), returnCode);

            _dispatcher = dispatcher;

            var rslt = new JVWatchEventResult();
            rslt.SetArguments();
            rslt.SetReturnCode(returnCode);
            return rslt;
        }

        public JVWatchEventCloseResult JVWatchEventClose()
        {
            VerboseApiCalling(_listener, this, nameof(JVLink), nameof(JVLink.JVWatchEventClose));
            var returnCode = _jvLink.JVWatchEventClose();
            VerboseApiCalled(_listener, this, nameof(JVLink), nameof(JVLink.JVWatchEventClose), returnCode);

            _dispatcher = null;

            var rslt = new JVWatchEventCloseResult();
            rslt.SetArguments();
            rslt.SetReturnCode(returnCode);
            return rslt;
        }

        private void _jvLink_JVEvtPay(string bstr)
        {
            _dispatcher?.Dispatch(JVDataSpec._0B12, bstr);
        }

        private void _jvLink_JVEvtJockeyChange(string bstr)
        {
            _dispatcher?.Dispatch(JVDataSpec._0B16, bstr);
        }

        private void _jvLink_JVEvtWeather(string bstr)
        {
            _dispatcher?.Dispatch(JVDataSpec._0B16, bstr);
        }

        private void _jvLink_JVEvtCourseChange(string bstr)
        {
            _dispatcher?.Dispatch(JVDataSpec._0B16, bstr);
        }

        private void _jvLink_JVEvtAvoid(string bstr)
        {
            _dispatcher?.Dispatch(JVDataSpec._0B16, bstr);
        }

        private void _jvLink_JVEvtTimeChange(string bstr)
        {
            _dispatcher?.Dispatch(JVDataSpec._0B16, bstr);
        }

        private void _jvLink_JVEvtWeight(string bstr)
        {
            _dispatcher?.Dispatch(JVDataSpec._0B11, bstr);
        }
    }
}
