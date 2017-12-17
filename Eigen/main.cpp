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
#include <Eigen/Dense>
#include <Eigen/Sparse>

using Eigen::MatrixXd;
using Eigen::VectorXd;
using Eigen::SparseMatrix;
using Eigen::SparseLU;

using namespace std;

/* Function Prototypes */
void CleanFiles();
int CountSize();
SparseMatrix<double> CopyMatrixToSparse(MatrixXd matrix, int size);
void ReadDataDoubleFromFile(int size);
/* End of prototypes */

int main(int argc, char** argv) {
	using namespace boost::algorithm;
	CleanFiles();
	ReadDataDoubleFromFile(CountSize());
	return 0;
}

void CleanFiles()
{
	ofstream file1,file2;
	file1.open ("PartialPivLu[C].txt", std::ofstream::out | std::ofstream::trunc);
	file2.open ("SparseLu[C].txt", std::ofstream::out | std::ofstream::trunc);
	file1.close();
	file2.close();
}

int CountSize()
{
	using namespace boost::algorithm;
	ifstream input("C:\\Users\\Patryk\\Desktop\\Data\\DataRange.txt");
	vector<string> tokens;	
	string line;
	getline(input, line);
	split(tokens, line, is_any_of(" "));
	int result = tokens.size() - 1;
	return result;
}

void ReadDataDoubleFromFile(int size)
{
	using namespace boost::algorithm;
	using Eigen::SparseLU;
	MatrixXd firstDoubleMatrix(size,size);
	VectorXd firstDoubleVector(size);
	int k = 0;
	int flag = 0;
	ifstream input("C:\\Users\\Patryk\\Desktop\\Data\\DataRange.txt");
	for( std::string line; getline( input, line ); )
	{	
		int j = 0;
		if (line.find("***") != string::npos)
		{
				flag++;
				k--;
		}
		else
		{
			vector<string> tokens;	
			split(tokens, line, is_any_of(" "));
			for(auto& s: tokens)
			{
				replace( s.begin(), s.end(), ',', '.');
				if (j==size)
				{ 
					break;
				}
				else
				{
					double temporary = atof(s.c_str());
					if (flag==0)
					{						
						firstDoubleMatrix(k,j) = temporary;
					} 
					else 
					{
						firstDoubleVector(k-size) = temporary;
					} 			
				}
				j++;
			}			
		}		
		k++;		
	}
	
	SparseMatrix<double> sparseMatrix = CopyMatrixToSparse(firstDoubleMatrix, size);
	SparseLU<SparseMatrix<double>> solver;
		
	auto start = std::chrono::high_resolution_clock::now();
	MatrixXd partialResult = firstDoubleMatrix.partialPivLu().solve(firstDoubleVector);
	auto finish = std::chrono::high_resolution_clock::now();
	double pivLuTime = std::chrono::duration_cast<std::chrono::nanoseconds>(finish - start).count();

	solver.compute(sparseMatrix);	
	start = std::chrono::high_resolution_clock::now();
	VectorXd sparseLu = solver.solve(firstDoubleVector);
	finish = std::chrono::high_resolution_clock::now();
	double sparseLuTime = std::chrono::duration_cast<std::chrono::nanoseconds>(finish - start).count();
	
	pivLuTime /= 1000000;
	sparseLuTime /= 1000000;
		
	std::ofstream outfile;
	outfile.open("C:\\Users\\Patryk\\Desktop\\Data\\Result5[C].txt", std::ios_base::app);
	outfile << "PartialPivLu: " << setprecision(16) << fixed << partialResult(0) << endl << "PartialPivLu Time: " << pivLuTime << endl;
	outfile << "\n";
	outfile << "SparseLu: " << setprecision(16) << fixed << sparseLu(0) << endl << "SparseLu Time: " << pivLuTime << endl;
	outfile << "\n";
	outfile << "*** **** *** *** *** ***\n";
	outfile.close();
	
}

SparseMatrix<double> CopyMatrixToSparse(MatrixXd matrix, int size) 
{
	SparseMatrix<double> smatrix(size, size);
	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size; j++)
		{
			smatrix.insert(i, j) = matrix(i, j);
		}
	}
	return smatrix;
}

namespace patch
{
    template < typename T > std::string to_string( const T& n )
    {
        std::ostringstream stm ;
        stm << n ;
        return stm.str() ;
    }
}