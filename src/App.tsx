import React, { useState, useEffect } from 'react';
import { Phone, Shield, LogOut, Trophy, Clock, Star, Users, Target, Zap } from 'lucide-react';

interface User {
  mobile: string;
  isVerified: boolean;
  sessionStart: Date;
}

type AuthState = 'login' | 'otp' | 'authenticated';

function App() {
  const [authState, setAuthState] = useState<AuthState>('login');
  const [mobile, setMobile] = useState('');
  const [otp, setOtp] = useState(['', '', '', '', '', '']);
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');
  const [sessionTime, setSessionTime] = useState<string>('');

  // Session timer
  useEffect(() => {
    let interval: NodeJS.Timeout;
    if (user) {
      interval = setInterval(() => {
        const elapsed = Date.now() - user.sessionStart.getTime();
        const minutes = Math.floor(elapsed / 60000);
        const seconds = Math.floor((elapsed % 60000) / 1000);
        setSessionTime(`${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`);
      }, 1000);
    }
    return () => clearInterval(interval);
  }, [user]);

  // Load session from localStorage
  useEffect(() => {
    const savedUser = localStorage.getItem('darkAuth_user');
    if (savedUser) {
      const parsedUser = JSON.parse(savedUser);
      setUser({
        ...parsedUser,
        sessionStart: new Date(parsedUser.sessionStart)
      });
      setAuthState('authenticated');
    }
  }, []);

  const validateMobile = (mobile: string): boolean => {
    const mobileRegex = /^[6-9]\d{9}$/;
    return mobileRegex.test(mobile);
  };

  const handleMobileSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    
    if (!validateMobile(mobile)) {
      setError('Please enter a valid 10-digit mobile number');
      return;
    }

    setIsLoading(true);
    
    // Simulate API call
    await new Promise(resolve => setTimeout(resolve, 1500));
    
    setIsLoading(false);
    setAuthState('otp');
  };

  const handleOtpChange = (index: number, value: string) => {
    if (value.length > 1) return;
    
    const newOtp = [...otp];
    newOtp[index] = value;
    setOtp(newOtp);

    // Auto-focus next input
    if (value && index < 5) {
      const nextInput = document.getElementById(`otp-${index + 1}`);
      nextInput?.focus();
    }
  };

  const handleKeyDown = (index: number, e: React.KeyboardEvent) => {
    if (e.key === 'Backspace' && !otp[index] && index > 0) {
      const prevInput = document.getElementById(`otp-${index - 1}`);
      prevInput?.focus();
    }
  };

  const handleOtpSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    
    const otpCode = otp.join('');
    if (otpCode.length !== 6) {
      setError('Please enter the complete 6-digit OTP');
      return;
    }

    setIsLoading(true);
    
    // Simulate API call
    await new Promise(resolve => setTimeout(resolve, 2000));
    
    if (otpCode === '123456') {
      const newUser: User = {
        mobile,
        isVerified: true,
        sessionStart: new Date()
      };
      
      setUser(newUser);
      localStorage.setItem('darkAuth_user', JSON.stringify(newUser));
      setAuthState('authenticated');
    } else {
      setError('Invalid OTP. Try 123456 for demo.');
    }
    
    setIsLoading(false);
  };

  const handleLogout = () => {
    localStorage.removeItem('darkAuth_user');
    setUser(null);
    setMobile('');
    setOtp(['', '', '', '', '', '']);
    setAuthState('login');
    setSessionTime('');
  };

  const LoginForm = () => (
    <div className="w-full max-w-md mx-auto">
      <div className="bg-white/95 backdrop-blur-xl rounded-3xl p-8 shadow-2xl border border-gray-200">
        <div className="text-center mb-8">
          <div className="w-20 h-20 bg-gradient-to-br from-blue-500 via-purple-600 to-green-500 rounded-2xl flex items-center justify-center mx-auto mb-6 shadow-lg">
            <Trophy className="w-10 h-10 text-white" />
          </div>
          <h1 className="text-3xl font-bold text-gray-800 mb-2">FantasyPro</h1>
          <p className="text-gray-600 mb-4">Your Ultimate Fantasy Sports Experience</p>
          
          {/* Sports Icons */}
          <div className="flex justify-center space-x-4 mb-6">
            <div className="w-10 h-10 bg-orange-500/20 rounded-full flex items-center justify-center border border-orange-500/30">
              <div className="w-6 h-6 bg-orange-500 rounded-full"></div>
            </div>
            <div className="w-10 h-10 bg-green-500/20 rounded-full flex items-center justify-center border border-green-500/30">
              <Target className="w-5 h-5 text-green-600" />
            </div>
            <div className="w-10 h-10 bg-blue-500/20 rounded-full flex items-center justify-center border border-blue-500/30">
              <Zap className="w-5 h-5 text-blue-600" />
            </div>
          </div>
        </div>

        <form onSubmit={handleMobileSubmit} className="space-y-6">
          <div className="text-center mb-6">
            <h2 className="text-xl font-semibold text-gray-800 mb-2">Join the Game</h2>
            <p className="text-gray-600 text-sm">Enter your mobile number to get started</p>
          </div>
          
          <div className="relative">
            <label className="block text-sm font-medium text-gray-700 mb-2">Mobile Number</label>
            <input
              type="tel"
              value={mobile}
              onChange={(e) => setMobile(e.target.value.replace(/\D/g, '').slice(0, 10))}
              className="w-full px-4 py-4 bg-gray-50 border-2 border-gray-300 rounded-xl text-gray-800 placeholder-gray-500 focus:outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-500/20 transition-all duration-300"
              placeholder="Enter mobile number"
              maxLength={10}
              required
            />
            <div className="absolute right-3 top-10 transform -translate-y-1/2">
              <Phone className="w-5 h-5 text-gray-400" />
            </div>
          </div>

          {error && (
            <div className="p-3 bg-red-50 border border-red-200 rounded-lg text-red-600 text-sm">
              {error}
            </div>
          )}

          <button
            type="submit"
            disabled={isLoading}
            className="w-full bg-gradient-to-r from-blue-600 via-purple-600 to-green-600 text-white font-semibold py-4 px-6 rounded-xl hover:from-blue-500 hover:via-purple-500 hover:to-green-500 focus:outline-none focus:ring-2 focus:ring-blue-500/50 transition-all duration-300 disabled:opacity-50 disabled:cursor-not-allowed shadow-lg"
          >
            {isLoading ? (
              <div className="flex items-center justify-center">
                <div className="w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin mr-2"></div>
                Sending OTP...
              </div>
            ) : (
              'Get Started'
            )}
          </button>
          
          {/* Features Preview */}
          <div className="mt-8 pt-6 border-t border-gray-200">
            <div className="grid grid-cols-2 gap-4 text-center">
              <div className="flex flex-col items-center">
                <div className="w-8 h-8 bg-blue-500/20 rounded-full flex items-center justify-center mb-2 border border-blue-500/30">
                  <Users className="w-4 h-4 text-blue-600" />
                </div>
                <span className="text-xs text-gray-600">Multi-Sport</span>
              </div>
              <div className="flex flex-col items-center">
                <div className="w-8 h-8 bg-green-500/20 rounded-full flex items-center justify-center mb-2 border border-green-500/30">
                  <Star className="w-4 h-4 text-green-600" />
                </div>
                <span className="text-xs text-gray-600">Live Contests</span>
              </div>
            </div>
          </div>
        </form>
      </div>
    </div>
  );

  const OtpForm = () => (
    <div className="w-full max-w-md mx-auto">
      <div className="bg-white/95 backdrop-blur-xl rounded-3xl p-8 shadow-2xl border border-gray-200">
        <div className="text-center mb-8">
          <div className="w-20 h-20 bg-gradient-to-br from-blue-500 via-purple-600 to-green-500 rounded-2xl flex items-center justify-center mx-auto mb-6 shadow-lg">
            <Shield className="w-10 h-10 text-white" />
          </div>
          <h1 className="text-3xl font-bold text-gray-800 mb-2">Verify Code</h1>
          <p className="text-gray-600">
            Enter the 6-digit code sent to<br />
            <span className="text-blue-600 font-semibold">+91 {mobile}</span>
          </p>
        </div>

        <form onSubmit={handleOtpSubmit} className="space-y-6">
          <div className="flex justify-center space-x-3">
            {otp.map((digit, index) => (
              <input
                key={index}
                id={`otp-${index}`}
                type="text"
                value={digit}
                onChange={(e) => handleOtpChange(index, e.target.value)}
                onKeyDown={(e) => handleKeyDown(index, e)}
                className="w-12 h-12 text-center text-xl font-bold bg-gray-50 border-2 border-gray-300 rounded-xl text-gray-800 focus:outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-500/20 transition-all duration-300"
                maxLength={1}
              />
            ))}
          </div>

          <div className="text-center">
            <p className="text-gray-500 text-sm mb-4">Demo OTP: 123456</p>
          </div>

          {error && (
            <div className="p-3 bg-red-50 border border-red-200 rounded-lg text-red-600 text-sm">
              {error}
            </div>
          )}

          <button
            type="submit"
            disabled={isLoading}
            className="w-full bg-gradient-to-r from-blue-600 via-purple-600 to-green-600 text-white font-semibold py-4 px-6 rounded-xl hover:from-blue-500 hover:via-purple-500 hover:to-green-500 focus:outline-none focus:ring-2 focus:ring-blue-500/50 transition-all duration-300 disabled:opacity-50 disabled:cursor-not-allowed shadow-lg"
          >
            {isLoading ? (
              <div className="flex items-center justify-center">
                <div className="w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin mr-2"></div>
                Verifying...
              </div>
            ) : (
              'Join FantasyPro'
            )}
          </button>

          <button
            type="button"
            onClick={() => setAuthState('login')}
            className="w-full text-gray-500 hover:text-gray-700 transition-colors duration-300 mt-4"
          >
            Back to Mobile Number
          </button>
        </form>
      </div>
    </div>
  );

  const Dashboard = () => (
    <div className="w-full max-w-2xl mx-auto">
      <div className="bg-white/95 backdrop-blur-xl rounded-3xl p-8 shadow-2xl border border-gray-200">
        <div className="text-center mb-8">
          <div className="w-24 h-24 bg-gradient-to-br from-blue-500 via-purple-600 to-green-500 rounded-2xl flex items-center justify-center mx-auto mb-6 shadow-lg">
            <Trophy className="w-12 h-12 text-white" />
          </div>
          <h1 className="text-4xl font-bold text-gray-800 mb-2">Welcome to FantasyPro!</h1>
          <p className="text-gray-600">Ready to dominate the fantasy leagues?</p>
        </div>

        <div className="space-y-6">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div className="bg-gray-50 rounded-xl p-6 border border-gray-200">
              <div className="flex items-center mb-3">
                <Phone className="w-5 h-5 text-blue-600 mr-2" />
                <h3 className="text-gray-800 font-semibold">Mobile Number</h3>
              </div>
              <p className="text-gray-600">+91 {user?.mobile}</p>
            </div>

            <div className="bg-gray-50 rounded-xl p-6 border border-gray-200">
              <div className="flex items-center mb-3">
                <Clock className="w-5 h-5 text-green-600 mr-2" />
                <h3 className="text-gray-800 font-semibold">Session Time</h3>
              </div>
              <p className="text-gray-600">{sessionTime}</p>
            </div>
          </div>

          <div className="bg-gradient-to-r from-green-50 to-blue-50 rounded-xl p-6 border border-green-200">
            <div className="flex items-center justify-between">
              <div>
                <h3 className="text-gray-800 font-semibold mb-1">Account Status</h3>
                <p className="text-green-400 text-sm flex items-center">
                  <div className="w-2 h-2 bg-green-500 rounded-full mr-2"></div>
                  Verified & Ready to Play
                </p>
              </div>
              <div className="w-12 h-12 bg-green-500/20 rounded-full flex items-center justify-center">
                <Shield className="w-6 h-6 text-green-600" />
              </div>
            </div>
          </div>

          {/* Quick Actions */}
          <div className="grid grid-cols-2 gap-4">
            <button className="bg-gradient-to-r from-blue-600 to-purple-600 text-white font-semibold py-4 px-6 rounded-xl hover:from-blue-500 hover:to-purple-500 transition-all duration-300 flex items-center justify-center">
              <Trophy className="w-5 h-5 mr-2" />
              Join Contest
            </button>
            <button className="bg-gradient-to-r from-green-600 to-blue-600 text-white font-semibold py-4 px-6 rounded-xl hover:from-green-500 hover:to-blue-500 transition-all duration-300 flex items-center justify-center">
              <Users className="w-5 h-5 mr-2" />
              My Teams
            </button>
          </div>

          <button
            onClick={handleLogout}
            className="w-full bg-gray-100 text-gray-700 font-semibold py-4 px-6 rounded-xl hover:bg-gray-200 focus:outline-none focus:ring-2 focus:ring-gray-300/50 transition-all duration-300 flex items-center justify-center border border-gray-300"
          >
            <LogOut className="w-5 h-5 mr-2" />
            Logout
          </button>
        </div>
      </div>
    </div>
  );

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 via-purple-50 to-green-50 flex items-center justify-center p-4">
      {/* Background Pattern */}
      <div className="absolute inset-0 opacity-10">
        <div className="absolute inset-0 bg-[radial-gradient(circle_at_25%_25%,#3b82f6_0%,transparent_70%)]"></div>
        <div className="absolute inset-0 bg-[radial-gradient(circle_at_75%_75%,#10b981_0%,transparent_70%)]"></div>
      </div>
      
      {/* Main Content */}
      <div className="relative z-10 w-full">
        {authState === 'login' && <LoginForm />}
        {authState === 'otp' && <OtpForm />}
        {authState === 'authenticated' && <Dashboard />}
      </div>
    </div>
  );
}

export default App;