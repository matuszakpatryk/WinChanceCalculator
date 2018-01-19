#include <iostream>
#include <fstream>
#include <typeinfo>
#include <cstdio>
#include <ctime>
#include <chrono>
#include <sstream>
#include <string>
#include <iomanip>
#include <boost/algorithm/string/split.hpp>
#include <boost/algorithm/string/classification.hpp>
#include <Eigen/OrderingMethods>
#include <Eigen/Dense>
#include <Eigen/Sparse>

using Eigen::MatrixXd;
using Eigen::VectorXd;
using Eigen::SparseMatrix;
using Eigen::SparseLU;

int ROZ = 1352;
using namespace std;

/* Function Prototypes */
SparseMatrix<double> CopyMatrixToSparse(MatrixXd matrix, int size);
double ReadDataDoubleFromFile();
/* End of prototypes */

int main(int argc, char** argv) {
	using namespace boost::algorithm;
	double sum = 0;
	for(int i=0; i<50; i++)
	{
		sum += ReadDataDoubleFromFile();
	}
	
	cout << "Srednia: " << sum/50 << endl;
	return 0;
}

double ReadDataDoubleFromFile()
{
	using namespace boost::algorithm;
	using Eigen::SparseLU;
	
	SparseMatrix<double> matrix(ROZ, ROZ);
	VectorXd doubleVector(ROZ);
	
	ifstream input("C:\\Users\\Patryk\\Desktop\\Data\\MatrixData13.txt");
	for( std::string line; getline( input, line ); )
	{	
		vector<string> tokens;	
		split(tokens, line, is_any_of(" "));
		if (tokens.size() == 3)
		{
			double i = atof(tokens[0].c_str());
			double j = atof(tokens[1].c_str());
			replace( tokens[2].begin(), tokens[2].end(), ',', '.');
			double value = atof(tokens[2].c_str());
			matrix.insert(i,j) = value;
		}
		else
		{
			continue;
		}
	}
	
	ifstream inputt("C:\\Users\\Patryk\\Desktop\\Data\\VectorData13.txt");
	for( std::string line; getline( inputt, line ); )
	{	
		vector<string> tokens;	
		split(tokens, line, is_any_of(" "));
		if (tokens.size() == 2)
		{
			double i = atof(tokens[0].c_str());
			replace( tokens[1].begin(), tokens[1].end(), ',', '.');
			double value = atof(tokens[1].c_str());
			doubleVector(i) = value;
		}else
		{
			continue;
		}	
	}
	
	matrix.makeCompressed();
	
	SparseLU<SparseMatrix<double>> solver;
	
	solver.analyzePattern(matrix);
	solver.factorize(matrix);	
	solver.compute(matrix);
	
	auto start = std::chrono::high_resolution_clock::now();
	VectorXd sparseLu = solver.solve(doubleVector);
	auto finish = std::chrono::high_resolution_clock::now();
	double sparseLuTime = std::chrono::duration_cast<std::chrono::nanoseconds>(finish - start).count();

	sparseLuTime /= 1000000;
	
	cout << "SparseLu: " << setprecision(16) << fixed << sparseLu(0) << endl << "SparseLu Time: " << sparseLuTime << endl;	
	return sparseLuTime;
}