#include<iostream>
#include<list>
#include<vector>
#include<gsl/gsl_rng.h>
#include<gsl/gsl_randist.h>
#include<set>
#include<ctime>
#include"coord_double.h"
#include"iohb_double.h"
#include"iotext_double.h"
#include<sys/stat.h>


using namespace std;

//Global Variables

const gsl_rng_type *T;
gsl_rng *pare=gsl_rng_alloc(gsl_rng_mt19937);
gsl_rng *retu=gsl_rng_alloc(gsl_rng_rand);
int hy_count = 0;
int dang_count = 0;

//Basic Web class which stores information about the random Web created

class web{
	public:
		int size;
		vector<vector<int> > in_links;
		vector<int> number_out_links;
		vector<int> dangling_pages;
		
	public:
		web(int n){
			size = n;
			for(int i=0;i<n;i++){
				vector<int> temp;
				in_links.push_back(temp);
				number_out_links.push_back(0);
				dangling_pages.push_back(1);

			}
	}

};

//Function that returns a random variable from Pareto Distribution of order "power"

int paretosample(int n, double power = 2.0){

	int v = n+1;
	while(v>n){
		v = gsl_ran_pareto(pare,power/2,1);
	}
	return v;
}

//Constructs a random Web with n pages where each page is linked to lk random
//pages The lk are independent and identically distributed random
//variables with a shifted and truncated Pareto probability mass 
//function p(l) proportional to 1/(l+1)^power

void return_web(int n,double power, web &g){
	dang_count = n;
	set<int> *unique_values = new set<int>;
	for(int i=0;i<n;i++){
		int lk = paretosample(n,power);
		while(lk==n){
			
			lk = paretosample(n,power)-1;
		}
		
		hy_count += lk;
		vector<int> values;
		
		set<int>::iterator set_it;
		
		while((int) (*unique_values).size()!=lk){
			int temp =gsl_rng_uniform_int(retu,n);
			if(temp!=i){
				(*unique_values).insert(temp);
			}
		}
		for(set_it=(*unique_values).begin();set_it != (*unique_values).end();set_it++){
			(values).push_back(*set_it);
		}
		(*unique_values).clear();
		
		g.in_links[i]= (values);
		vector<int>::iterator it;
		
		for(it = (values).begin();it<(values).end();it++){
			if(g.number_out_links[*it] ==0){
				g.dangling_pages[*it]=0;
				dang_count--;
			}
			
			g.number_out_links[*it] += 1;
			
		}
	}
	free(unique_values);
	
}

//Calcualates the tolerance factor for the PageRank so that
//we know have we reached an accurate Vector or not

double Cal_Tol(MV_Vector_double P1,MV_Vector_double P0){
	int len = P1.size();
	double tol=0.0;
	double temp;

	for(int i=0;i<len;i++){
		temp = P1[i]-P0[i];
		if(temp<0)
		    tol += (-1)*temp;
		else
		    tol += temp;

	}
	return tol;
}

//Multiplies the current PageRank Vector with the dangling_pages vector to obtain a scaler

double Comp_Scale(MV_Vector_double P, web current_web){
	double result = 0;
	for(int i =0;i<current_web.size;i++){
	    result += P[i]*current_web.dangling_pages[i];
	}
	return result;
}

/* 
    MAIN
*/

int main(int argc, char* argv[]){
    
	if(argc != 4){
		cout<<"Usage ./part <number of pages> <distribution power> <Part No>";
		exit(1);
	}
	
	struct stat buffer;
	char choice= 'N';
	Coord_Mat_double hyperlink;
	char* filename = "matrix.txt";
	web current_web = web(1);
	int flag;
	int N = 1;
	
	if(stat(filename,&buffer)==0 ){
		cout<<"A Matrix already exists.\n Do you want this matrix as Hyperlink matrix?\n The PageRank of Part1 will be computed if used and FILE will be OVERWRITTEN\n Press Y or N:";
		cin>>choice;
		if(choice == 'Y' || choice == 'y'){
		    
			readHB_mat(filename,&hyperlink);
			flag = 1;
			N = hyperlink.dim(0);
		}
	}
	if(stat(filename,&buffer) || choice == 'N'||choice=='n'){
	    
		cout<<"########################\n";
		cout<<"#                      #\n";
		cout<<"#Generating Random Web #\n";
		cout<<"#                      #\n";
		cout<<"########################\n";

		gsl_rng_set(retu,time(NULL));
		gsl_rng_set(pare,time(NULL));
		
		N = atoi(argv[1]);
		flag = atoi(argv[3]);
		current_web = web(N);
		
		return_web(atoi(argv[1]),atoi(argv[2]),current_web);
		
		gsl_rng_free(retu);
		gsl_rng_free(pare);
		
		vector<int>::iterator it;
		double *val;
		int *row_ind,*col_ind,ind_count=0;
		
		if(flag ==1||flag ==3){
		    
		    val = (double*)malloc(sizeof(double)*hy_count);
		    row_ind = (int*)malloc(sizeof(int)*hy_count);
		    col_ind = (int*)malloc(sizeof(int)*hy_count);
		}
		else if(flag == 2){
		    
		    val = (double*)malloc(sizeof(double)*(dang_count*N + hy_count));
		    row_ind = (int*)malloc(sizeof(int)*(dang_count*N + hy_count));
		    col_ind = (int*)malloc(sizeof(int)*(dang_count*N + hy_count));
		}
		else{
		    exit(1);
		}
		
		vector<bool>::iterator itb;
		
//Uncomment the following code if you want to see the randomnly generated Web

/*		
		cout << "Final Web:\n";
		for(int i=0;i<current_web.size;i++){
			cout<<"\nPage #"<<i<<endl;
			cout<<"Number of in_links:[ ";
			for(int j=0;j<(int)current_web.in_links[i].size();j++){
				
				cout<<current_web.in_links[i][j]<<",";
			}
			cout<<"]\n";
			cout<<"number of out_links"<<current_web.number_out_links[i]<<"  ";
			cout<<"is it dangling:"<<current_web.dangling_pages[i]<<"  ";
		}
*/

		int web_size = current_web.size;
		for(int i=0;i<web_size;i++){
			if(current_web.dangling_pages[i] == 1 && (flag == 2)){
				for(int j=0;j<web_size;j++){
					row_ind[ind_count] = i;
					col_ind[ind_count] = j;
					val[ind_count] = (double)1.0/(double)N;
					ind_count++;
				}
			}
			else{
			    int in_links =(int)current_web.in_links[i].size();
			    int *out_page_no;
			    for(int j=0;j<in_links;j++){
				    out_page_no = &current_web.in_links[i][j];
				    col_ind[ind_count]=*out_page_no;
				    row_ind[ind_count]=i;
				    val[ind_count]=(double)(1.0/current_web.number_out_links[*out_page_no]);
				    ind_count++;
				    out_page_no=NULL;
			    }
			    free(out_page_no);
			    
			}
			
		}

		
		hyperlink  = Coord_Mat_double(N,N,ind_count,val,row_ind,col_ind);
		free(val);
		free(row_ind);
		free(col_ind);
		writeHB_mat(filename,hyperlink);
	}
	
//After Generating the Web we now calculate the PageRank vector

//Uncomment to display the Hyperlink matrix
//	cout<<hyperlink;

	cout<<"#############################\n";
	cout<<"#                           #\n";
	cout<<"#Calculating PageRank Vector#\n";
	cout<<"#                           #\n";
	cout<<"#############################\n";
	
	clock_t start,end;
	start = clock();
	if(flag == 1 || flag == 2){
	    MV_Vector_double P = MV_Vector_double (N,(double)1/(double)N);
	    int tol = 99999.9999;
	    MV_Vector_double temp = hyperlink*P;

	    tol = Cal_Tol(P,temp);
	    while(tol>0.0001){
		    MV_Vector_double temp2 = hyperlink*temp;
		    tol =Cal_Tol(temp,temp2);

		    temp = temp2;
	    }
//Uncomment the following to Print the final Matrix

//	    cout<<"printing final Matrix\n";
//	    cout<<temp;
	}
	else if(flag == 3){

	    MV_Vector_double P = MV_Vector_double (N,(double)1/(double)N);
	    MV_Vector_double e = MV_Vector_double(N,1);
	    double scaler = Comp_Scale(P,current_web);

	    double tol = 99999.9999;
	    double alpha = 0.85;
	    cout<<"Enter the teleportation factor: ";
	    cin>>alpha;
	    if(alpha > 1.0 || alpha < 0.0){
		cout <<"Factor OUT OF RANGE\nNow 0.85 will be used\n";
		alpha = 0.85;
	    }
	    MV_Vector_double temp = alpha*(hyperlink*P) + (alpha*scaler + 1 - alpha)/N*e;
	    
	    tol = Cal_Tol(P,temp);

	    while(tol>0.00001){
		    MV_Vector_double temp2 = alpha*(hyperlink*temp) + (alpha*Comp_Scale(temp,current_web) + 1 - alpha)/N*e;
		    tol =Cal_Tol(temp,temp2);

		    temp = temp2;
	    }
//Uncomment the following to Print the final Matrix

//	    cout<<"printing final Matrix\n";
//	    cout<<temp;
	    
	}
	end = clock();
	cout<<(double)(end-start)/CLOCKS_PER_SEC;
	return 0;	
}
