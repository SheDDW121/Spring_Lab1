#include "pch.h"
#include <time.h>
#include <mkl.h>
#include <math.h>
#include <iostream>
#include <chrono>
#include <thread>
#include <algorithm>

extern "C"  _declspec(dllexport)
bool Get_MKL_TIMES(MKL_INT n, const double* a, double* y, double* z, double* p, double& time_HA, double& time_EP, double& time_NO_MKL, MKL_INT FUN) {
	try 
	{

		if (FUN == 0) {

			//if (n < 500000) // with LA
			//	vmdTan(n, a, y, VML_LA); // первый раз для того, чтобы не было такого, как у меня было, что первая функция, которую запускаешь работает медленее
			auto start = std::chrono::high_resolution_clock::now();
			vmdTan(n, a, y, VML_HA);
			auto end = std::chrono::high_resolution_clock::now();

			std::chrono::duration<double, std::milli> float_ms = end - start;
			time_HA = float_ms.count() / 1000;

			start = std::chrono::high_resolution_clock::now();
			vmdTan(n, a, z, VML_EP);
			end = std::chrono::high_resolution_clock::now();

			float_ms = end - start;
			time_EP = float_ms.count() / 1000;

			start = std::chrono::high_resolution_clock::now();
			for (int i = 0; i < n; i++) {
				p[i] = tan(a[i]);
			}
			end = std::chrono::high_resolution_clock::now();

			float_ms = end - start;
			time_NO_MKL = float_ms.count() / 1000;
			return 0;
		}
		else {
			//if (n < 500000) // with LA
			//	vmdErfInv(n, a, y, VML_LA); // для того, чтобы первый раз время на всякие инициализации не считалось

			auto start = std::chrono::high_resolution_clock::now();
			vmdErfInv(n, a, y, VML_HA);
			auto end = std::chrono::high_resolution_clock::now();

			std::chrono::duration<double, std::milli> float_ms = end - start;
			time_HA = float_ms.count() / 1000;

			start = std::chrono::high_resolution_clock::now();
			vmdErfInv(n, a, z, VML_EP);
			end = std::chrono::high_resolution_clock::now();

			float_ms = end - start;
			time_EP = float_ms.count() / 1000;

			start = std::chrono::high_resolution_clock::now();
			for (int i = 0; i < n; i++) {
				vmdErfInv(1, &a[i], &p[i], VML_HA);
			}
			end = std::chrono::high_resolution_clock::now();

			float_ms = end - start;
			time_NO_MKL = float_ms.count() / 1000;
			return 0;
		}
	}
	catch (...) {
		throw;
	}
}
extern "C"  _declspec(dllexport)
bool Get_MKL_ACCUR(MKL_INT n, const double* a, double* y, double* z, double& max, double* arg_max, MKL_INT FUN) {
	try
	{

		if (FUN == 0) {
			vmdTan(n, a, y, VML_HA);
			vmdTan(n, a, z, VML_EP);
		}

		else {
			vmdErfInv(n, a, y, VML_HA);
			vmdErfInv(n, a, z, VML_EP);
		}

		max = -1;
		double Arg_max = -1;
		arg_max[0] = arg_max[1] = arg_max[2] = -1;
		for (int i = 0; i < n; i++) {
			if (abs(y[i] - z[i]) > max) {
				Arg_max = a[i];
				arg_max[0] = a[i];
				arg_max[1] = y[i];
				arg_max[2] = z[i];
				max = abs(y[i] - z[i]);
			}
		}
	}
	catch (...) {
		throw;
	}
	return 0;
}