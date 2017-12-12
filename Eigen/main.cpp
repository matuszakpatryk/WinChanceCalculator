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

using Eigen::MatrixXd;
using Eigen::VectorXd;
using namespace std;

/* Function Prototypes */
void CleanFiles();
int CountSize();
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
	ifstream input("C:\\Users\\pmatusza\\Documents\\MobaXterm\\home\\Studia\\Algorytmy\\Zad3\\Data\\Data\\DataRangeDouble.txt");
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
	cout << "Size" << size << endl;
	MatrixXd firstDoubleMatrix(size,size);
	MatrixXd secondDoubleMatrix(size,size);
	MatrixXd thirdDoubleMatrix(size,size);
	VectorXd firstDoubleVector(size);
	int k = 0;
	int flag = 0;
	ifstream input("C:\\Users\\pmatusza\\Documents\\MobaXterm\\home\\Studia\\Algorytmy\\Zad3\\Data\\Data\\DataRangeDouble.txt");
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
					else if (flag==1)
					{
						secondDoubleMatrix(k-size,j) = temporary;
					} 
					else if (flag==2)
					{
						thirdDoubleMatrix(k-(size*2),j) = temporary;
					}
					else
					{
						firstDoubleVector(k-(size*3)) = temporary;
					}				
				}
				j++;
			}			
		}		
		k++;		
	}
	
	cout << "First Matrix" << endl << setprecision(3) << fixed << firstDoubleMatrix << endl;
	cout << "Second Matrix" << endl << setprecision(3) << fixed << secondDoubleMatrix << endl;
	cout << "Third  Matrix" << endl << setprecision(3) << fixed << thirdDoubleMatrix << endl;
	cout << "First Vector" << endl << setprecision(3) << fixed << firstDoubleVector << endl;
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