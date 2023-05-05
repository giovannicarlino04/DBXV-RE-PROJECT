class Iggy:
    def add_char_slot(file_path, char_data):
        initiator = bytes.fromhex('03')
        terminator = bytes.fromhex('06')
        jco_id = b'JCO'

        with open(file_path, 'rb') as f:
            data = f.read()

        # Find the last occurrence of the JCO character ID
        last_jco_offset = data.rfind(jco_id)
        if last_jco_offset == -1:
            raise ValueError('Could not find JCO character ID')

        # Calculate the offset to insert the new character
        last_char_end_offset = data.find(terminator, last_jco_offset)
        if last_char_end_offset == -1:
            raise ValueError('Could not find terminator byte')

        # Calculate the new character data size
        new_char_size = len(initiator) + len(char_data) + len(terminator)

        # Insert the new character data
        new_data = data[:last_char_end_offset] + initiator + char_data.encode() + data[last_char_end_offset:]
        with open(file_path, 'wb') as f:
            f.write(new_data)