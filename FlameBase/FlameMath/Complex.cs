using System;
using System.Windows;

namespace FlameBase.FlameMath
{
    public class Complex
    {
        private const double Tolerance = 1E-300;
        private double _perFix;
        public double Im;
        public double Re;
        public double SaveIm;
        public double SaveRe;

        public Complex()
        {
            Re = 0.0;
            Im = 0.0;
            SaveRe = 0.0;
            SaveIm = 0.0;
            _perFix = 0.0;
        }

        public Complex(double re)
        {
            Re = re;
            Im = 0.0;
            SaveRe = 0.0;
            SaveIm = 0.0;
            _perFix = 0.0;
        }

        public Complex(int n)
        {
            Re = n;
            Im = 0.0;
            SaveRe = 0.0;
            SaveIm = 0.0;
            _perFix = 0.0;
        }

        public Complex(Point p)
        {
            Re = p.X;
            Im = p.Y;
            SaveRe = 0.0;
            SaveIm = 0.0;
            _perFix = 0.0;
        }

        public Complex(double re, double im)
        {
            Re = re;
            Im = im;
            SaveRe = 0.0;
            SaveIm = 0.0;
            _perFix = 0.0;
        }

        public Complex(Complex complex)
        {
            Re = complex.Re;
            Im = complex.Im;
            SaveRe = 0.0;
            SaveIm = 0.0;
            _perFix = 0.0;
        }

        public void One()
        {
            Re = 1.0;
            Im = 0.0;
        }

        public void ImOne()
        {
            Re = 0.0;
            Im = 1.0;
        }

        public void Zero()
        {
            Re = 0.0;
            Im = 0.0;
        }

        public void Copy(Complex complex)
        {
            Re = complex.Re;
            Im = complex.Im;
        }

        public void Flip()
        {
            var im = Im;
            var re = Re;
            Re = im;
            Im = re;
        }

        public void Conj()
        {
            Im = -Im;
        }

        public void Neg()
        {
            Re = -Re;
            Im = -Im;
        }

        public double Sig()
        {
            if (Math.Abs(Re) < Tolerance) return 0.0;
            if (Re > 0.0) return 1.0;
            return -1.0;
        }

        public double Sig2()
        {
            if (Re >= 0.0) return 1.0;
            return -1.0;
        }

        public double Mag2()
        {
            return Re * Re + Im * Im;
        }

        public double Mag2Eps()
        {
            return Re * Re + Im * Im + 1.0E-20;
        }

        public double MagInv()
        {
            var mag2 = Mag2();
            return mag2 < 1.0E-100 ? 1.0 : 1.0 / mag2;
        }

        public void Save()
        {
            SaveRe = Re;
            SaveIm = Im;
        }

        public void Restore()
        {
            Re = SaveRe;
            Im = SaveIm;
        }

        public void Switch()
        {
            var saveRe = SaveRe;
            var saveIm = SaveIm;
            SaveRe = Re;
            SaveIm = Im;
            Re = saveRe;
            Im = saveIm;
        }

        public void Keep(Complex complex)
        {
            SaveRe = complex.Re;
            SaveIm = complex.Im;
        }

        public Complex Recall()
        {
            return new Complex(SaveRe, SaveIm);
        }

        public void NextPow()
        {
            Mul(Recall());
        }

        public void Sqr()
        {
            var re = Re * Re - Im * Im;
            var im = 2.0 * Re * Im;
            Re = re;
            Im = im;
        }

        public void Recip()
        {
            var magInv = MagInv();
            Re *= magInv;
            Im = -Im * magInv;
        }

        public void Scale(double n)
        {
            Re *= n;
            Im *= n;
        }

        public void Mul(Complex complex)
        {
            if (Math.Abs(complex.Im) < Tolerance)
            {
                Scale(complex.Re);
                return;
            }

            var re = Re * complex.Re - Im * complex.Im;
            var im = Re * complex.Im + Im * complex.Re;
            Re = re;
            Im = im;
        }

        public void Div(Complex complex)
        {
            var n = Im * complex.Im + Re * complex.Re;
            var n2 = Im * complex.Re - Re * complex.Im;
            var magInv = complex.MagInv();
            Re = n * magInv;
            Im = n2 * magInv;
        }

        public void DivR(Complex complex)
        {
            var n = complex.Im * Im + complex.Re * Re;
            var n2 = complex.Im * Re - complex.Re * Im;
            var magInv = MagInv();
            Re = n * magInv;
            Im = n2 * magInv;
        }

        public void Add(Complex complex)
        {
            Re += complex.Re;
            Im += complex.Im;
        }

        public void AMean(Complex complex)
        {
            Add(complex);
            Scale(0.5);
        }

        public void RootMeanS(Complex complex)
        {
            var complex2 = new Complex(complex);
            complex2.Sqr();
            Sqr();
            Add(complex2);
            Scale(0.5);
            Pow(0.5);
        }

        public void GMean(Complex complex)
        {
            Mul(complex);
            Pow(0.5);
        }

        public void Heronian(Complex complex)
        {
            var complex2 = new Complex(this);
            complex2.GMean(complex);
            Add(complex);
            Add(complex2);
            Scale(0.3333333333333333);
        }

        public void HMean(Complex complex)
        {
            var n = complex.Re + Re;
            var n2 = complex.Im + Im;
            var n3 = 0.5 * (n * n + n2 * n2);
            if (Math.Abs(n3) < Tolerance)
            {
                Zero();
                return;
            }

            var n4 = 1.0 / n3;
            var mag2 = Mag2();
            var mag3 = complex.Mag2();
            if (Math.Abs(mag2 * mag3) < Tolerance)
            {
                Zero();
                return;
            }

            Re = n4 * (Re * mag3 + complex.Re * mag2);
            Im = n4 * (Im * mag3 + complex.Im * mag2);
        }

        public void Sub(Complex complex)
        {
            Re -= complex.Re;
            Im -= complex.Im;
        }

        public void SubR(Complex complex)
        {
            Re = complex.Re - Re;
            Im = complex.Im - Im;
        }

        public void Inc()
        {
            ++Re;
        }

        public void Dec()
        {
            --Re;
        }

        public void PerFix(double n)
        {
            _perFix = 3.141592653589793 * n;
        }

        public void Pow(double n)
        {
            if (Math.Abs(n) < Tolerance)
            {
                One();
                return;
            }

            var fabs = Math.Abs(n);
            if (n < 0.0) Recip();
            if (Math.Abs(fabs - 0.5) < Tolerance)
            {
                Sqrt();
                return;
            }

            if (Math.Abs(fabs - 1.0) < Tolerance) return;
            if (Math.Abs(fabs - 2.0) < Tolerance)
            {
                Sqr();
                return;
            }

            var toP = ToP();
            toP.Re = Math.Pow(toP.Re, fabs);
            toP.Im *= fabs;
            Copy(toP.UnP());
        }

        public double Radius()
        {
            return Math.Sqrt(Re * Re + Im * Im);
        }

        public double Arg()
        {
            return _perFix + Math.Atan2(Im, Re);
        }

        public Complex ToP()
        {
            return new Complex(Radius(), Arg());
        }

        public Complex UnP()
        {
            return new Complex(Re * Math.Cos(Im), Re * Math.Sin(Im));
        }

        public void Norm()
        {
            Scale(Math.Sqrt(MagInv()));
        }

        public void Exp()
        {
            Re = Math.Exp(Re);
            Copy(UnP());
        }

        public void SinH()
        {
            var n = 1.0;
            Re = Math.Exp(Re);
            var n2 = n / Re;
            var n3 = 0.5 * (Re - n2);
            var n4 = n3 + n2;
            Re = Math.Cos(Im) * n3;
            Im = Math.Sin(Im) * n4;
        }

        public void Sin()
        {
            Flip();
            SinH();
            Flip();
        }

        public void CosH()
        {
            var n = 1.0;
            Re = Math.Exp(Re);
            var n2 = n / Re;
            var n3 = 0.5 * (Re - n2);
            Re = Math.Cos(Im) * (n3 + n2);
            Im = Math.Sin(Im) * n3;
        }

        public void Cos()
        {
            Flip();
            CosH();
            Flip();
        }

        public void Sqrt()
        {
            var radius = Radius();
            Im = (Im < 0.0 ? -1.0 : 1.0) * Math.Sqrt(0.5 * (radius - Re));
            Re = Math.Sqrt(0.5 * (radius + Re));
            if (_perFix < 0.0) Neg();
        }

        public void Log()
        {
            Copy(new Complex(0.5 * Math.Log(Mag2Eps()), Arg()));
        }

        public void LMean(Complex complex)
        {
            var complex2 = new Complex(this);
            var complex3 = new Complex(this);
            complex2.Sub(complex);
            complex3.Div(complex);
            complex3.Log();
            complex2.Div(complex3);
            Copy(complex2);
        }

        public void AtanH()
        {
            var complex = new Complex(this);
            complex.Dec();
            complex.Neg();
            Inc();
            Div(complex);
            Log();
            Scale(0.5);
        }

        public void AsinH()
        {
            var complex = new Complex(this);
            complex.Sqr();
            complex.Inc();
            complex.Pow(0.5);
            Add(complex);
            Log();
        }

        public void AcosH()
        {
            var complex = new Complex(this);
            complex.Sqr();
            complex.Dec();
            complex.Pow(0.5);
            Add(complex);
            Log();
        }

        public void AcotH()
        {
            Recip();
            AtanH();
        }

        public void AsecH()
        {
            Recip();
            AsinH();
        }

        public void AcosecH()
        {
            Recip();
            AcosH();
        }

        public void Atan()
        {
            Flip();
            AtanH();
            Flip();
        }

        public void Asin()
        {
            Flip();
            AsinH();
            Flip();
        }

        public void Acos()
        {
            Flip();
            AsinH();
            Flip();
            Re = 1.5707963267948966 - Re;
            Im = -Im;
        }

        public void CPow(Complex complex)
        {
            if (Math.Abs(complex.Im) < Tolerance)
            {
                Pow(complex.Re);
                return;
            }

            Log();
            Mul(complex);
            Exp();
        }
    }
}