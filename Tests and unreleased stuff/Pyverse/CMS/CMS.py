import struct


class CMS_File:
    def __init__(self):
        self.CMS_Entries = []

    def save_to_bytes(self):
        byte_list = []
        byte_list.extend(struct.pack('i', len(self.CMS_Entries)))
        byte_list.extend(struct.pack('i', 0))  # Placeholder for offset

        offset = 16 + 80 * len(self.CMS_Entries)
        for entry in self.CMS_Entries:
            byte_list.extend(struct.pack('i', int(entry.Index)))
            byte_list.extend(entry.Str_04.encode('utf-8').ljust(8, b'\x00'))
            byte_list.extend(struct.pack('q', entry.I_08))
            byte_list.extend(struct.pack('i', entry.I_16))
            byte_list.extend(struct.pack('H', entry.I_20))
            byte_list.extend(struct.pack('H', entry.I_22))
            byte_list.extend(struct.pack('H', entry.I_24))
            byte_list.extend(struct.pack('H', entry.I_26))
            byte_list.extend(struct.pack('i', entry.I_28))
            byte_list.extend(struct.pack('i', offset))
            offset += len(entry.Str_32) + 1
            byte_list.extend(entry.Str_32.encode('utf-8'))
            byte_list.extend(b'\x00')
            byte_list.extend(struct.pack('i', offset))
            offset += len(entry.Str_36) + 1
            byte_list.extend(entry.Str_36.encode('utf-8'))
            byte_list.extend(b'\x00')
            byte_list.extend(struct.pack('i', offset))
            offset += len(entry.Str_44) + 1
            byte_list.extend(entry.Str_44.encode('utf-8'))
            byte_list.extend(b'\x00')
            byte_list.extend(struct.pack('i', offset))
            offset += len(entry.Str_48) + 1
            byte_list.extend(entry.Str_48.encode('utf-8'))
            byte_list.extend(b'\x00')
            byte_list.extend(struct.pack('i', offset))
            offset += len(entry.Str_56) + 1
            byte_list.extend(entry.Str_56.encode('utf-8'))
            byte_list.extend(b'\x00')
            byte_list.extend(struct.pack('i', offset))
            offset += len(entry.Str_60) + 1
            byte_list.extend(entry.Str_60.encode('utf-8'))
            byte_list.extend(b'\x00')
            byte_list.extend(struct.pack('i', offset))
            offset += len(entry.Str_64) + 1
            byte_list.extend(entry.Str_64.encode('utf-8'))
            byte_list.extend(b'\x00')
            byte_list.extend(struct.pack('i', offset))
            offset += len(entry.Str_68) + 1
            byte_list.extend(entry.Str_68.encode('utf-8'))
            byte_list.extend(b'\x00')
            byte_list.extend(struct.pack('i', offset))
            offset += len(entry.Str_80) + 1
            byte_list.extend(entry.Str_80.encode('utf-8'))
            byte_list.extend(b'\x00')

        # Update offset
        byte_list[4:8] = struct.pack('i', offset)

        return bytes(byte_list)

    def sort_entries(self):
        self.CMS_Entries.sort(key=lambda x: int(x.Index))

    def get_entry(self, id):
        for entry in self.CMS_Entries:
            if entry.Index == id:
                return entry
        return None

    def save_binary(self, path):
        with open(path, 'wb') as file:
            file.write(self.save_to_bytes())

    def chara_id_to_chara_code(self, chara_id):
        chara_id_str = str(chara_id)
        entry = next((e for e in self.CMS_Entries if e.Index == chara_id_str), None)
        if entry is not None:
            return entry.Str_04
        return ''

    def chara_code_to_chara_id(self, chara_code):
        entry = next((e for e in self.CMS_Entries if e.Str_04 == chara_code), None)
        if entry is not None:
            return int(entry.Index)
        return -1


class CMS_Entry:
    def __init__(self):
        self.Index = ''  # string
        self.Str_04 = ''  # string
        self.I_08 = 0  # int64
        self.I_16 = 0  # int32
        self.I_20 = 0  # uint16
        self.I_22 = 0  # uint16
        self.I_24 = 0  # uint16
        self.I_26 = 0  # uint16
        self.I_28 = 0  # int32
        self.Str_32 = ''  # string
        self.Str_36 = ''  # string
        self.Str_44 = ''  # string
        self.Str_48 = ''  # string
        self.Str_56 = ''  # string
        self.Str_60 = ''  # string
        self.Str_64 = ''  # string
        self.Str_68 = ''  # string
        self.Str_80 = ''  # string