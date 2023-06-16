/// <copyright>3Shape A/S</copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Ivi.Visa;
using Ivi.Visa.FormattedIO;
using System.Windows;


namespace _3ShapeMainboardLSTester.Models
{
    public class Multimeter
    {
        //Multimeter VISA adress
        string _visaAdress = empty;

        //Multimeter name
        string _name = empty;

        //Crate a message model for communication with DC Electronic Load
        MessageBasedFormattedIO _formattedIO;

        // Create a connection (session) to the instrument
        IMessageBasedSession _session;

        //Command variable 
        string _command = empty;

        //Variable for storing Multimeter response 
        string _idnResponse;

        //Auxiliary variables
        bool _isConnected = false;
        Int32 _lastTestResult;

        //Test configuration variables - set to default
        public double measuredValueMinLimit = 0;
        public double measuredValueMaxLimit = 1.3;
        double _startCurrent = 1.17;
        double _maxCurrent = 1.24;
        double _currentIncreasement = 0.001;
        bool _checkVoltageAfterTest = false;



        // KtEL30000 Commands
        const string enableInput = "INP ON, ";
        const string disableInput = "INP OFF, ";
        const string identify = "*IDN?";
        const string setCCMode = "FUNC CURR, ";
        const string checkMode = "FUNC? ";
        const string CCMode = "CURR";
        const string readCurrent = "CURR?";
        const string readRealVoltage = "MEAS:VOLT?";
        const string readRealCurrent = "MEAS:CURR?";
        const string empty = "";
        string setCurrent(double currentValue)
        {
            return "CURR " + currentValue;
        }


        public bool IsConnected()
        {
            return _isConnected;
        }

        public string[] checkConnection()
        {
            string[] returnValue = new string[2];
            returnValue[0] = "Connection - FAIL";
            returnValue[1] = "";
            string arguments = _visaAdress + " " + "connect";
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _fileName,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            if (_visaAdress is not null && _visaAdress.Length > 0)
            {
                //add chcecking if there is file and add try catch
                proc.Start();
                while (!proc.StandardOutput.EndOfStream)
                {
                    returnValue[1] = returnValue[1] + proc.StandardOutput.ReadLine() + "; ";
                }
                if (proc.ExitCode == 111)
                {
                    returnValue[0] = "Connection - PASS";
                    _isConnected = true;
                }else
                {
                    _isConnected = false;
                }
            }
            return returnValue;
        }
        public void setVisaAdress(string name)
        {
            _visaAdress = name;
        }

        public void setControlAppName(string name)
        {
            _fileName = name;
        }

        public string[] test()
        {
            string[] returnValue = new string[2];
            returnValue[0] = "Test - FAIL";
            returnValue[1] = "";
            string arguments = _visaAdress + " " + "test" + " " + _startCurrent + " " + _maxCurrent + " " + _currentIncreasement + " " + _checkVoltageAfterTest;
            double testResult;
            _lastTestResult = false;
            _lastTestResultValue = 0;
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _fileName,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            if (_visaAdress is not null && _visaAdress.Length > 0)
            {
                //add chcecking if there is file and add try catch
                proc.Start();
                while (!proc.StandardOutput.EndOfStream)
                {
                    returnValue[1] = returnValue[1] + proc.StandardOutput.ReadLine() + "; ";
                }
                resultFirst = proc.ExitCode;
                resulSecond = (double)proc.ExitCode;
                resulThird = Convert.ToDouble(resultFirst);
                resulFourth = Convert.ToDouble(resultFirst) / divider;
                resulFifth = proc.ExitCode;
                testResult = ((double)proc.ExitCode) / divider;
                _lastTestResultValue = testResult;
                if (testResult >= measuredValueMinLimit && testResult <= measuredValueMaxLimit)
                {
                    returnValue[0] = "Test - PASS, result: " + testResult;
                    _lastTestResult = true;
                }
            }
            return returnValue;
        }
        public void setTestLimits(double minTestLimit, double maxTestLimit)
        {
            measuredValueMinLimit = minTestLimit;
            measuredValueMaxLimit = maxTestLimit;
        }
        public double getTestResultValue()
        {
            return _lastTestResultValue;
        }

        public bool getTestResult()
        {
            return _lastTestResult;
        }

        public void setTestParameters(double startCurrent, double maxCurrent, double currentIncreasement, bool checkVoltageAfterTest)
        {
            _startCurrent = startCurrent;
            _maxCurrent = maxCurrent;
            _currentIncreasement = currentIncreasement;
            _checkVoltageAfterTest = checkVoltageAfterTest;
        }

        public bool StartSeassion()
        {
            bool returnValue = false;
            _isConnected = false;
            try
            {
                _session = GlobalResourceManager.Open(_visaAdress) as IMessageBasedSession;
            }
            catch (NativeVisaException visaException)
            {
                MessageBox.Show("Couldn't connect to multimeter. Error is: {0}", visaException.ToString());
            }

            _formattedIO = new MessageBasedFormattedIO(_session);

            // For Serial and TCP/IP socket connections enable the read Termination Character, or read's will timeout
            if (_session.ResourceName.Contains("ASRL") || _session.ResourceName.Contains("SOCKET"))
                _session.TerminationCharacterEnabled = true;

            // Send the identify command and read the response as strings
            _command = identify;
            _formattedIO.WriteLine(_command);
            _idnResponse = _formattedIO.ReadLine();
            if(_idnResponse != null && _idnResponse.Contains(_name))
            {
                returnValue = true;
                _isConnected = true;
            }
            return returnValue;
        }

        public void EndSession()
        {
            _session.Dispose();
            _isConnected = false;
        }
    }
}