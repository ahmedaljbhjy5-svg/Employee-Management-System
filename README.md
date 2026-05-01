1. نظرة عامة على المشروع
نظام إدارة الموظفين هو تطبيق C# يُطبّق مبادئ البرمجة الغرضية التوجه (OOP) بشكل كامل. يدير النظام أنواعاً مختلفة من الموظفين (مدير، مطور، متدرب) ضمن أقسام، مع تطبيق جميع المبادئ المطلوبة: التجريد، الوراثة، تعدد الأشكال، التغليف، التفويض، الأحداث، والفئات الساكنة.

2. مبادئ OOP المطبّقة
2.1 التجريد (Abstraction)
تم تطبيق التجريد من خلال واجهة (Interface) وفئة مجردة (Abstract Class):

الوصف	الاسم	العنصر
تحدد 3 عمليات أساسية: CalculateSalary(), DisplayInfo(), Work()	IEmployeeActions	Interface
تطبق IEmployeeActions وتحتوي على خصائص مشتركة	Employee	Abstract Class
كل فئة فرعية تحسب الراتب بطريقتها	CalculateSalary()	دالة مجردة
كل فئة فرعية تنفذ عملها بطريقتها	Work()	دالة مجردة

2.2 الوراثة وتعدد الأشكال (Inheritance & Polymorphism)
ثلاث فئات فرعية ترث من الفئة المجردة Employee، كل منها تعيد تعريف الدوال المجردة بطريقتها الخاصة. فئة Department تحتوي على List<Employee> وتعالج جميع الموظفين بنفس الكود، مما يظهر تعدد الأشكال في وقت التشغيل.

سلوك Work()	حساب الراتب	ترث من	الفئة
إدارة الفريق واتخاذ القرارات	BaseSalary + Bonus	Employee	Manager
كتابة الكود وتطوير البرمجيات	HoursWorked * HourlyRate	Employee	Developer
التعلم والمساعدة في القسم	BaseSalary (المكافأة)	Employee	Intern

مثال على تعدد الأشكال — فئة Department تكرر على List<Employee> وتستدعي Work() وDisplayInfo() وCheckSalary() على كل كائن، ويتم تنفيذ النسخة الصحيحة تلقائياً حسب النوع الفعلي.

2.3 التغليف (Encapsulation)
جميع الحقول في الفئة Employee والفئات الفرعية معرّفة كـ private مع بادئة _ (مثل: _name, _age, _baseSalary). الوصول يتم عبر Properties مع منطق تحقق.

التحقق	نوع الوصول	الخاصية	الحقل
غير فارغ، طول >= 2	للقراءة فقط خارجياً	Name { get; private set; }	_name
بين 18 و 65	للقراءة فقط خارجياً	Age { get; private set; }	_age
يجب أن يكون > 0	protected write	BaseSalary { get; protected set; }	_baseSalary
لا يتغير بعد الإنشاء	للقراءة فقط (غير قابل للتعديل)	Id { get; private set; }	(auto)

2.4 التفويض والأحداث (Delegates & Events)
تم تعريف delegate باسم SalaryAlertHandler داخل فئة Employee. عند استدعاء CheckSalary() وكان الراتب أكبر من 10,000 $، يُطلق الحدث OnHighSalary ويتم استدعاء الدالة المشتركة HandleHighSalaryAlert() التي تطبع تنبيهاً ملوناً.

الدور	الاسم	العنصر
يحدد التوقيع: (string name, double salary)	SalaryAlertHandler	Delegate
يُطلق عندما يكون الراتب > 10,000 $	OnHighSalary	Event
يطبع تنبيهاً أصفر في الـ Console	HandleHighSalaryAlert()	دالة المعالجة
تحسب الراتب وتطلق الحدث إذا لزم	CheckSalary()	دالة التشغيل

2.5 الفئات الساكنة (Static Classes & Members)
الفئة الساكنة DataValidator توفر دوال تحقق بدون الحاجة لإنشاء كائنات منها. كما تحتوي فئة Employee على خاصيتين ساكنتين لتتبع الحالة العامة لجميع الكائنات.

الغرض	الاسم	الفئة	النوع
التحقق من Name, Age, Salary, Hours	DataValidator	DataValidator	Static Class
يتحقق أن الاسم غير فارغ وطوله >= 2	IsValidName(string)	DataValidator	Static Method
يتحقق أن العمر بين 18 و 65	IsValidAge(int)	DataValidator	Static Method
يتحقق أن الراتب > 0	IsValidSalary(double)	DataValidator	Static Method
يحسب عدد الموظفين المنشأين	Count { get; private set; }	Employee	Static Property
يجمع مجموع رواتب كل الموظفين	TotalSalaryBudget { get; private set; }	Employee	Static Property

3. مخطط UML
يوضح المخطط التالي العلاقات بين جميع الفئات في النظام:

<<interface>>  IEmployeeActions
+ CalculateSalary() : double
+ DisplayInfo() : void
+ Work() : void
         |  implements
         v
<<abstract>>  Employee
- _id, _name, _age, _baseSalary  [private]
+ Id { get; private set; }  [read-only]
+ Name, Age, BaseSalary  [Properties]
+ Count : int  [static]
+ TotalSalaryBudget : double  [static]
+ OnHighSalary : event SalaryAlertHandler
+ CalculateSalary()  [abstract]
+ Work()  [abstract]
+ DisplayInfo(), CheckSalary()
    |           |           |
    v           v           v
 Manager    Developer    Intern
 +Bonus     +HoursWorked +Department
 +Calc..()  +HourlyRate  +Calc...()
 +Work()    +Calc...()   +Work()
            +Work()

 Department                  DataValidator <<static>>
 -_employees:List<Employee>  +IsValidName(string):bool
 +AddEmployee(Employee)      +IsValidAge(int):bool
 +ShowAllEmployees()         +IsValidSalary(double):bool

4. شرح التفويض والأحداث — الدور العملي
يعمل التفويض والحدث كآلية إشعار بين الفئات دون ربط مباشر بينها. إليك خطوات التنفيذ:

الإجراء	الخطوة
تعريف delegate باسم SalaryAlertHandler بمعاملين: اسم الموظف والراتب	1. التعريف
إعلان حدث OnHighSalary من نوع SalaryAlertHandler في فئة Employee	2. الإعلان
في Main()، كل موظف يشترك: m1.OnHighSalary += HandleHighSalaryAlert	3. الاشتراك
عند استدعاء CheckSalary()، إذا كان الراتب > 10,000 $ يُطلق الحدث	4. التشغيل
تنفيذ HandleHighSalaryAlert() وطباعة تنبيه أصفر على الـ Console	5. المعالجة

الفائدة العملية: فئة Employee لا تحتاج أن تعرف من سيتعامل مع التنبيه. أي دالة تطابق توقيع الـ delegate يمكنها الاشتراك، مما يجعل التصميم مرناً وغير مترابط.
