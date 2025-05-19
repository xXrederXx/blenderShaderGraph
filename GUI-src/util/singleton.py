"""Contains Singleton class"""


class Singleton(type):
    """
    To make any class a singleton do this:
        MyClass(metaclass=Singleton)
    Then you can just call MyClass() everywere
    """

    _instances = {}

    def __call__(cls, *args, **kwargs):
        if cls not in cls._instances:
            cls._instances[cls] = super(Singleton, cls).__call__(*args, **kwargs)
        return cls._instances[cls]
