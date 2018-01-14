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

int ROZ = 1800;
double sum = 0;
double countXTime, countXWhileTime, countXFirstForTime, countXSecondForTime, valueForTime, valueFor1Time, countResultTime, countResultForTime, checkVectorTime, checkVectorForTime = 0;
int countXwhile, countXFirstFor,countXSecondFor,valueFor,countResult, countResultFor, countResultIf, checkVector, checkVectorFor, checkVectorIf, checkZero = 0;
using namespace std;

/* Function Prototypes */
void CleanFiles();
int CountSize();
SparseMatrix<double> CopyMatrixToSparse(MatrixXd matrix, int size);
void ReadDataDoubleFromFile(int size);
bool CheckIsDoubleZero(double number);
VectorXd GaussSeidelMethod(MatrixXd matrix, VectorXd vectorB, int size, int numberOfIterations);
void Seidel(VectorXd bVector, int numberOfIterations, MatrixXd matrix, double *xVector);
void SetDefaultVector(int numberOfColumns, double *vector);
double InverseNumber(double x);
bool CheckIsVectorSameAfterNextIteration(double *lastVector, double *vector);
double CountResultOfActionsForGivenRowOfXVector(int numberOfColumns, int rowNumber, double *xVector, MatrixXd matrix);
double ValueFromIterationForRow(double *xVector, MatrixXd matrix, int rowNumber, VectorXd bVector);
void CountXVector(double *xVector, int numberOfIterations, MatrixXd matrix,  VectorXd bVector);
/* End of prototypes */

int main(int argc, char** argv) {
	using namespace boost::algorithm;
	//CleanFiles();
	ReadDataDoubleFromFile(CountSize());
	return 0;
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

void CleanFiles()
{
	ofstream file1,file2;
	file1.open ("PartialPivLu[C].txt", std::ofstream::out | std::ofstream::trunc);
	file2.open ("SparseLu[C].txt", std::ofstream::out | std::ofstream::trunc);
	file1.close();
	file2.close();
}

void ReadDataDoubleFromFile(int size)
{
	using namespace boost::algorithm;
	using Eigen::SparseLU;
	MatrixXd firstDoubleMatrix(ROZ,ROZ);
	VectorXd firstDoubleVector(ROZ);
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
	
	
	cout << "Wczytalem" << endl;
	SparseMatrix<double> sparseMatrix = CopyMatrixToSparse(firstDoubleMatrix, ROZ);
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
	
	double result[ROZ];
	
	SetDefaultVector(size,result);
	
	cout << "seidel time" << endl;
	//start = std::chrono::high_resolution_clock::now();
	//CountXVector(result, 100, firstDoubleMatrix, firstDoubleVector);
	//finish = std::chrono::high_resolution_clock::now();
	start = std::chrono::high_resolution_clock::now();
	VectorXd seidel = GaussSeidelMethod(firstDoubleMatrix, firstDoubleVector, size, 100);
	finish = std::chrono::high_resolution_clock::now();
	cout << "seidel time ended" << endl;
	
	double seidelTime = std::chrono::duration_cast<std::chrono::nanoseconds>(finish - start).count();
	
	cout << "CountXVector Time: " << setprecision(16) << fixed << countXTime << endl;
	cout << "CountXVector while count : " << countXwhile << endl;
	cout << "CountXVector first for time: " << setprecision(16) << fixed << countXFirstForTime/1000000 << endl;
	cout << "CountXVector first for count: " << setprecision(16) << fixed << countXFirstFor << endl;
	cout << "CountXVector second for time: " << setprecision(16) << fixed << countXSecondForTime/1000000 << endl;
	cout << "CountXVector second for count: " << setprecision(16) << fixed << countXSecondFor << endl;
	cout << "ValueFor Time: " << setprecision(16) << fixed << valueForTime/1000000 << endl;
	cout << "ValueFor1 Time: " << setprecision(16) << fixed << valueFor1Time/1000000 << endl;
	cout << "ValueFor Count: " << setprecision(16) << fixed << valueFor << endl;
	cout << "CountResult Time: " << setprecision(16) << fixed << countResultTime/1000000 << endl;
	cout << "CountResult Count: " << setprecision(16) << fixed << countResult << endl;
	cout << "CountResult For count: " << setprecision(16) << fixed << countResultFor << endl;
	cout << "CountResult If count: " << setprecision(16) << fixed << countResultIf << endl;
	cout << "CheckIsVectorSame Time: " << setprecision(16) << fixed << checkVectorTime/1000000 << endl;
	cout << "CheckIsVectorSame Count: " << setprecision(16) << fixed << checkVector << endl;
	cout << "CheckIsVectorSame For Count: " << setprecision(16) << fixed << checkVectorFor << endl;
	cout << "CheckIsDoubleZero count: " << setprecision(16) << fixed << checkZero << endl;
	cout << "Seidel Time: " << setprecision(16) << fixed << seidelTime/1000000 << endl;
	
	//for(int i=0; i < size ; i++)
	//{
	//	cout << "i: " << i << " value: " << result[i] << endl;
	//}
		
	std::ofstream outfile;
	outfile.open("C:\\Users\\Patryk\\Desktop\\Data\\Result10[C].txt", std::ios_base::app);
	outfile << "PartialPivLu: " << setprecision(16) << fixed << partialResult(0) << endl << "PartialPivLu Time: " << pivLuTime/1000000 << endl;
	outfile << "\n";
	outfile << "SparseLu: " << setprecision(16) << fixed << sparseLu(0) << endl << "SparseLu Time: " << sparseLuTime/1000000 << endl;
	outfile << "\n";
	outfile << "Seidel: " << setprecision(16) << fixed << result[0] << endl << "Seidel: " << seidelTime/1000000 << endl;
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

VectorXd GaussSeidelMethod(MatrixXd matrix, VectorXd vectorB, int size, int numberOfIterations) 
{
	MatrixXd U(size, size);
	MatrixXd D(size, size);
	MatrixXd L(size, size);
	VectorXd x1(size);
	VectorXd x2(size);

    double x1norm = 0;
    double x2norm = 0;

    for (int i = 0; i < size; i++)
    {
        for (int j = 0; j < size; j++)
        {
            if (i == j)
            {
                D(i, j) = InverseNumber(matrix(i, j));
            }
            else if (i > j)
            {
                L(i, j) = matrix(i, j);
            }
            else if (i < j)
            {
                U(i, j) = matrix(i, j);
            }
        }
    }
    for (int k = 0; k < numberOfIterations; k++)
    {
        for (int i = 0; i < size; i++)
        {
            x1(i) = vectorB(i) * D(i, i);
            for (int j = 0; j < i; j++)
            {
                x1(i) -= D(i,i) * L(i, j) * x1(j);
            }
            for (int j = i + 1; j < size; j++)
            {
                x1(i) -= D(i, i) * U(i, j) * x1(j);
            }
        }
        for(int j = 0; j < size; j++)
        {
            x1norm += x1(j) * x1(j);
            x2norm += x2(j) * x2(j);
        }

        x2norm = sqrt(x2norm);
        x1norm = sqrt(x1norm);

        if (!CheckIsDoubleZero(x1norm - x2norm))
        {
            cout << "Gauss Seidel breaks on iteration nr - " << k << endl;
            break;
        }

        for (int j = 0; j < size; j++)
        {
            x2(j) = x1(j);
        }
    }
    return x1;
}

double InverseNumber(double x)
{
    if (abs(x) < 1e-14)
        return 0;

    return 1 / x;
}

void CountXVector(double *xVector, int numberOfIterations, MatrixXd matrix,  VectorXd bVector)
{
	auto startt = std::chrono::high_resolution_clock::now();
	
		int numberOfRows = ROZ;	
		double lastIterationXVector[ROZ];
		int countXwhile = 1;		
		while (true)
		{
			auto start = std::chrono::high_resolution_clock::now();
			
				for(int i=0; i<ROZ; i++)
				{
					countXFirstFor++;
					lastIterationXVector[i] = xVector[i];
				}
				
			auto finish = std::chrono::high_resolution_clock::now();
			countXFirstForTime = std::chrono::duration_cast<std::chrono::nanoseconds>(finish - start).count();
		
			start = std::chrono::high_resolution_clock::now();
			
				for (int j = 0; j < numberOfRows; j++)
				{
					countXSecondFor++;
					xVector[j] = ValueFromIterationForRow(xVector, matrix, j, bVector);
				}
			
			finish = std::chrono::high_resolution_clock::now();
			countXSecondForTime = std::chrono::duration_cast<std::chrono::nanoseconds>(finish - start).count();
			
			if (CheckIsVectorSameAfterNextIteration(lastIterationXVector, xVector))
			{
				break;
			}
			countXwhile++;
		}
	
	auto finishh = std::chrono::high_resolution_clock::now();
	countXTime = std::chrono::duration_cast<std::chrono::nanoseconds>(finishh - startt).count();

}

void SetDefaultVector(int numberOfColumns, double *vector)
{
    for (int i = 0; i < numberOfColumns; i++)
    {
        vector[i] = 0;
    }
}

bool CheckIsVectorSameAfterNextIteration(double *lastVector, double *vector)
{
	checkVector++;
	auto start = std::chrono::high_resolution_clock::now();
    for (int i = 0; i < ROZ; i++)
    {
		checkVectorFor++;
		double temp = lastVector[i] - vector[i];
        if (!CheckIsDoubleZero(temp))
        {
			return false;
        }
    }
 
    auto finish = std::chrono::high_resolution_clock::now();
	checkVectorTime = std::chrono::duration_cast<std::chrono::nanoseconds>(finish - start).count();
    return true;
}
 
bool CheckIsDoubleZero(double number)
{
	checkZero++;
    if (abs(number) <= 0.0000000001)
    {
        return true;
    }

    return false;
}

double CountResultOfActionsForGivenRowOfXVector(int numberOfColumns, int rowNumber, double *xVector, MatrixXd matrix)
{
	double result = 0;
	auto start = std::chrono::high_resolution_clock::now();
	
	countResult++;
    for (int i = 0; i < numberOfColumns; i++)
    {
		countResultFor++;
        if (i != rowNumber)
        {
			countResultIf++;
            result += matrix(rowNumber,i) * xVector[i];
        }
    }
	
	auto finish = std::chrono::high_resolution_clock::now();
	countResultTime = std::chrono::duration_cast<std::chrono::nanoseconds>(finish - start).count();
	
    return result;
}

double ValueFromIterationForRow(double *xVector, MatrixXd matrix, int rowNumber, VectorXd bVector)
{
	valueFor++;
    int numberOfColumns = ROZ;
	
	auto start = std::chrono::high_resolution_clock::now();	
    double resultOfActionsForGivenRowOfXVector = CountResultOfActionsForGivenRowOfXVector(numberOfColumns, rowNumber, xVector, matrix);
	auto finish = std::chrono::high_resolution_clock::now();
	valueForTime = std::chrono::duration_cast<std::chrono::nanoseconds>(finish - start).count();
	
	start = std::chrono::high_resolution_clock::now();	
    resultOfActionsForGivenRowOfXVector = (-resultOfActionsForGivenRowOfXVector + bVector[rowNumber]) / matrix(rowNumber,rowNumber);
	finish = std::chrono::high_resolution_clock::now();
	valueFor1Time = std::chrono::duration_cast<std::chrono::nanoseconds>(finish - start).count();	
	

    return resultOfActionsForGivenRowOfXVector;

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