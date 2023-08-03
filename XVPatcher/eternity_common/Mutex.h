#ifndef ___MUTEX_H___
#define ___MUTEX_H___

#include <windows.h>

class Mutex
{
private:

	void *handle;
	
	Mutex(const Mutex &);
	Mutex& operator=(const Mutex &);	

public:

	inline Mutex()
	{
		handle = (void *)CreateMutex(nullptr, false, nullptr);
	}
	
	inline ~Mutex()
	{
		CloseHandle((HANDLE)handle);
	}
	
	inline void Lock()
    {
        WaitForSingleObject((HANDLE)handle, INFINITE);
	}
	
	inline void Unlock()
	{
		ReleaseMutex((HANDLE)handle);
	}
};

class MutexLocker
{
private:

	Mutex *mutex;
	
	MutexLocker();	
	MutexLocker(const MutexLocker &);	
	MutexLocker& operator=(const MutexLocker &);

public:

	inline MutexLocker(Mutex *mutex) : mutex(mutex)
	{
		mutex->Lock();
	}
	
	inline ~MutexLocker()
	{
		mutex->Unlock();
	}
};

#endif // ___MUTEX_H___
