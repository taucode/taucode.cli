(defblock :name checkout :is-top t
	(worker
		:description "Checkouts a branch."
		:worker-name checkout
		:verb "checkout"
		:usage-samples (
			"git checkout branch-name"
			"git checkout -b new-branch master"
			))

	(idle :name options)

	(opt
		(alt
			(multi-text
				:classes key
				:values "-q" "--quiet"
				:alias quiet
				:action option
				:description "Be quiet")

			(multi-text
				:classes key integer-text
				:values "-2" "--ours"
				:alias ours
				:action option
				:description "Use ours")

			(multi-text
				:classes key integer-text
				:values "-3" "--theirs"
				:alias theirs
				:action option
				:description "Use theirs")

			(fallback :name bad-option-fallback)
		)
	)

	(idle :links options next)

	(alt
		(seq
			(multi-text
				:classes key
				:values "-b"
				:alias new-branch
				:description "Start new branch"
				:action option)
			(some-text
				:classes path
				:alias new-branch-name
				:action argument
				:description "New branch name"
				:doc-subst "new branch name"
			)
			(some-text
				:classes path
				:alias base-branch-name
				:action argument
				:description "Base branch name"
				:doc-subst "base branch name"
			)
		)

		(some-text
			:classes path
			:alias existing-branch-name
			:action argument
			:description "Existing branch name"
			:doc-subst "existing branch name"
		)
	)

	(end)
)
