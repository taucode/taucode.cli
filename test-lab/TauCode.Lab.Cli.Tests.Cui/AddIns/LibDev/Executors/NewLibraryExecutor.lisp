(defblock :name new-library :is-top t
	(executor
		:executor-name new-library
		:verb "new"
		:description "Changes current directory"
		:usage-samples (
			"libdev new TauCode.Foo"
		)
	)

	(seq
		(multi-text
			:classes key
			:values "-n" "--name"
			:alias name
			:action key
			:is-mandatory t)
		(some-text
			:classes path ; TODO: class should be 'type-name' or something.
			:action value
			:description "Library name"
			:doc-subst "library name"))

	(opt
		(multi-text
			:classes key
			:values "-d" "--directory"
			:alias directory
			:action key)
		(some-text
			:classes path string
			:action value
			:description "Directory"
			:doc-subst "directory"))

	(end)
)
